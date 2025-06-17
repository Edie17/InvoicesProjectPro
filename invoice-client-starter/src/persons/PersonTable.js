import React, { useState } from "react";
import { Link } from "react-router-dom";
import ConfirmModal from "../components/ConfirmModal";

const PAGE_SIZE = 10;

const PersonTable = ({ label, items, deletePerson }) => {
    const [sortDir, setSortDir] = useState("asc");
    const [page, setPage] = useState(1);
    const [pendingDeleteId, setPendingDeleteId] = useState(null);

    const sorted = [...items].sort((a, b) => {
        const av = a.name.toLowerCase(), bv = b.name.toLowerCase();
        if (av < bv) return sortDir === "asc" ? -1 : 1;
        if (av > bv) return sortDir === "asc" ? 1 : -1;
        return 0;
    });

    const totalPages = Math.max(1, Math.ceil(sorted.length / PAGE_SIZE));
    const paginated = sorted.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);

    return (
        <div>
            <p className="text-muted">{label} <strong>{items.length}</strong></p>

            <ConfirmModal
                show={pendingDeleteId !== null}
                message="Opravdu chcete tuto osobu smazat? Tato akce je nevratná."
                onConfirm={() => { deletePerson(pendingDeleteId); setPendingDeleteId(null); }}
                onCancel={() => setPendingDeleteId(null)}
            />

            <div className="table-responsive">
                <table className="table table-hover table-bordered align-middle">
                    <thead className="table-dark">
                        <tr>
                            <th>#</th>
                            <th
                                onClick={() => { setSortDir(d => d === "asc" ? "desc" : "asc"); setPage(1); }}
                                style={{ cursor: "pointer", userSelect: "none" }}
                            >
                                Jméno <span className="ms-1" style={{ fontSize: "0.7rem" }}>{sortDir === "asc" ? "▲" : "▼"}</span>
                            </th>
                            <th>Akce</th>
                        </tr>
                    </thead>
                    <tbody>
                        {paginated.map((item, index) => (
                            <tr key={item._id}>
                                <td className="text-muted">{(page - 1) * PAGE_SIZE + index + 1}</td>
                                <td><strong>{item.name}</strong></td>
                                <td>
                                    <div className="btn-group btn-group-sm">
                                        <Link to={"/persons/show/" + item._id} className="btn btn-outline-info">Zobrazit</Link>
                                        <Link to={"/persons/edit/" + item._id} className="btn btn-outline-warning">Upravit</Link>
                                        <button
                                            onClick={() => setPendingDeleteId(item._id)}
                                            className="btn btn-outline-danger"
                                        >
                                            Smazat
                                        </button>
                                    </div>
                                </td>
                            </tr>
                        ))}
                        {paginated.length === 0 && (
                            <tr>
                                <td colSpan={3} className="text-center text-muted py-4">Žádné osoby</td>
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
                <Link to={"/persons/create"} className="btn btn-success">+ Nová osoba</Link>
            </div>
        </div>
    );
};

export default PersonTable;
