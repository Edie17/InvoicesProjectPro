import React from "react";
import { Link } from "react-router-dom";
import { dateStringFormatter } from "../utils/dateStringFormatter";

/**
 * Component for displaying invoices in a table format.
 * 
 * @param {Object} props - Component properties
 * @param {string} props.label - Label to display before the count of items
 * @param {Array} props.items - Array of invoice objects to display
 * @param {Function} props.deleteInvoice - Function to call when deleting an invoice
 */
const InvoiceTable = ({ label, items, deleteInvoice }) => {
    return (
        <div>
            <p>
                {label} {items.length}
            </p>

            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Číslo faktury</th>
                        <th>Vystaveno</th>
                        <th>Splatnost</th>
                        <th>Produkt</th>
                        <th>Částka</th>
                        <th colSpan={3}>Akce</th>
                    </tr>
                </thead>
                <tbody>
                    {items.map((item, index) => (
                        <tr key={index + 1}>
                            <td>{index + 1}</td>
                            <td>{item.invoiceNumber}</td>
                            <td>{dateStringFormatter(item.issued)}</td>
                            <td>{dateStringFormatter(item.dueDate)}</td>
                            <td>{item.product}</td>
                            <td>{item.price} Kč</td>
                            <td>
                                <div className="btn-group">
                                    <Link
                                        to={"/invoices/show/" + item._id}
                                        className="btn btn-sm btn-info"
                                    >
                                        Zobrazit
                                    </Link>
                                    <Link
                                        to={"/invoices/edit/" + item._id}
                                        className="btn btn-sm btn-warning"
                                    >
                                        Upravit
                                    </Link>
                                    <button
                                        onClick={() => deleteInvoice(item._id)}
                                        className="btn btn-sm btn-danger"
                                    >
                                        Odstranit
                                    </button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <Link to={"/invoices/create"} className="btn btn-success">
                Nová faktura
            </Link>
        </div>
    );
};

export default InvoiceTable;