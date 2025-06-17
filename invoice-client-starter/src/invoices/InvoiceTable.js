import React, { useState } from "react";
import { Link } from "react-router-dom";
import { dateStringFormatter } from "../utils/dateStringFormatter";
import formatPrice from "../utils/formatPrice";
import ConfirmModal from "../components/ConfirmModal";

const PAGE_SIZE = 10;

const SortIcon = ({ field, sortField, sortDir }) => {
    if (sortField !== field) return <span className="text-muted ms-1" style={{ fontSize: "0.7rem" }}>⇅</span>;
    return <span className="ms-1" style={{ fontSize: "0.7rem" }}>{sortDir === "asc" ? "▲" : "▼"}</span>;
};

const InvoiceTable = ({ label, items, deleteInvoice }) => {
    const [sortField, setSortField] = useState("invoiceNumber");
    const [sortDir, setSortDir] = useState("desc");
    const [page, setPage] = useState(1);
    const [pendingDeleteId, setPendingDeleteId] = useState(null);

    const handleSort = (field) => {
        if (sortField === field) {
            setSortDir(d => d === "asc" ? "desc" : "asc");
        } else {
            setSortField(field);
            setSortDir("asc");
        }
        setPage(1);
    };

    const sorted = [...items].sort((a, b) => {
        let av = a[sortField], bv = b[sortField];
        if (typeof av === "string") av = av.toLowerCase(), bv = bv.toLowerCase();
        if (av < bv) return sortDir === "asc" ? -1 : 1;
        if (av > bv) return sortDir === "asc" ? 1 : -1;
        return 0;
    });

    const totalPages = Math.max(1, Math.ceil(sorted.length / PAGE_SIZE));
    const paginated = sorted.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);

    const thProps = (field) => ({
        onClick: () => handleSort(field),
        style: { cursor: "pointer", userSelect: "none" },
    });

    return (
        <div>
            <p className="text-muted">{label} <strong>{items.length}</strong></p>

            <ConfirmModal
                show={pendingDeleteId !== null}
                message="Opravdu chcete tuto fakturu smazat? Tato akce je nevratná."
                onConfirm={() => { deleteInvoice(pendingDeleteId); setPendingDeleteId(null); }}
                onCancel={() => setPendingDeleteId(null)}
            />

            <div className="table-responsive">
                <table className="table table-hover table-bordered align-middle">
                    <thead className="table-dark">
                        <tr>
                            <th>#</th>
                            <th {...thProps("invoiceNumber")}>
                                Číslo faktury <SortIcon field="invoiceNumber" sortField={sortField} sortDir={sortDir} />
                            </th>
                            <th {...thProps("issued")}>
                                Vystaveno <SortIcon field="issued" sortField={sortField} sortDir={sortDir} />
                            </th>
                            <th {...thProps("dueDate")}>
                                Splatnost <SortIcon field="dueDate" sortField={sortField} sortDir={sortDir} />
                            </th>
                            <th {...thProps("product")}>
                                Produkt <SortIcon field="product" sortField={sortField} sortDir={sortDir} />
                            </th>
                            <th {...thProps("price")}>
                                Částka <SortIcon field="price" sortField={sortField} sortDir={sortDir} />
                            </th>
                            <th>Akce</th>
                        </tr>
                    </thead>
                    <tbody>
                        {paginated.map((item, index) => (
                            <tr key={item._id}>
                                <td className="text-muted">{(page - 1) * PAGE_SIZE + index + 1}</td>
                                <td><strong>{item.invoiceNumber}</strong></td>
                                <td>{dateStringFormatter(item.issued)}</td>
                                <td>{dateStringFormatter(item.dueDate)}</td>
                                <td>{item.product}</td>
                                <td className="text-end fw-semibold">{formatPrice(item.price)}</td>
                                <td>
                                    <div className="btn-group btn-group-sm">
                                        <Link to={"/invoices/show/" + item._id} className="btn btn-outline-info">Zobrazit</Link>
                                        <Link to={"/invoices/edit/" + item._id} className="btn btn-outline-warning">Upravit</Link>
                                        {deleteInvoice && (
                                            <button
                                                onClick={() => setPendingDeleteId(item._id)}
                                                className="btn btn-outline-danger"
                                            >
                                                Smazat
                                            </button>
                                        )}
                                    </div>
                                </td>
                            </tr>
                        ))}
                        {paginated.length === 0 && (
                            <tr>
                                <td colSpan={7} className="text-center text-muted py-4">Žádné faktury</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>

            {totalPages > 1 && (
                <nav>
                    <ul className="pagination pagination-sm justify-content-center">
                        <li className={`page-item ${page === 1 ? "disabled" : ""}`}>
                            <button className="page-link" onClick={() => setPage(p => p - 1)}>«</button>
                        </li>
                        {Array.from({ length: totalPages }, (_, i) => i + 1).map(p => (
                            <li key={p} className={`page-item ${p === page ? "active" : ""}`}>
                                <button className="page-link" onClick={() => setPage(p)}>{p}</button>
                            </li>
                        ))}
                        <li className={`page-item ${page === totalPages ? "disabled" : ""}`}>
                            <button className="page-link" onClick={() => setPage(p => p + 1)}>»</button>
                        </li>
                    </ul>
                </nav>
            )}

            <div className="mt-2">
                <Link to={"/invoices/create"} className="btn btn-success me-2">+ Nová faktura</Link>
                <Link to={"/invoices/filter"} className="btn btn-outline-secondary">Filtrovat</Link>
            </div>
        </div>
    );
};

export default InvoiceTable;
