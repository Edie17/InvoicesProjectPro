import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { apiGet } from "../utils/api";
import { dateStringFormatter } from "../utils/dateStringFormatter";

/**
 * Component for displaying detailed information about a specific invoice.
 */
const InvoiceDetail = () => {
    const { id } = useParams();
    const [invoice, setInvoice] = useState({});

    useEffect(() => {
        apiGet("/api/invoices/" + id)
            .then((data) => setInvoice(data))
            .catch((error) => {
                console.error("Error fetching invoice data:", error);
            });
    }, [id]);

    return (
        <>
            <div>
                <h1>Detail faktury</h1>
                <hr />
                <h3>Faktura č. {invoice.invoiceNumber}</h3>
                <p>
                    <strong>Vystaveno:</strong>
                    <br />
                    {invoice.issued && dateStringFormatter(invoice.issued)}
                </p>
                <p>
                    <strong>Splatnost:</strong>
                    <br />
                    {invoice.dueDate && dateStringFormatter(invoice.dueDate)}
                </p>
                <p>
                    <strong>Produkt:</strong>
                    <br />
                    {invoice.product}
                </p>
                <p>
                    <strong>Částka:</strong>
                    <br />
                    {invoice.price} Kč
                </p>
                <p>
                    <strong>DPH:</strong>
                    <br />
                    {invoice.vat}%
                </p>
                <p>
                    <strong>Prodejce:</strong>
                    <br />
                    {invoice.seller?.name}
                </p>
                <p>
                    <strong>Kupující:</strong>
                    <br />
                    {invoice.buyer?.name}
                </p>
                <p>
                    <strong>Poznámka:</strong>
                    <br />
                    {invoice.note}
                </p>
            </div>
        </>
    );
};

export default InvoiceDetail;