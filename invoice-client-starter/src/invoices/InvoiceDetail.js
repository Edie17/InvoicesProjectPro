import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { apiGet } from "../utils/api";
import { dateStringFormatter } from "../utils/dateStringFormatter";
import formatPrice from "../utils/formatPrice";
import Breadcrumb from "../components/Breadcrumb";

const InvoiceDetail = () => {
    const { id } = useParams();
    const [invoice, setInvoice] = useState({});

    useEffect(() => {
        apiGet("/api/invoices/" + id)
            .then((data) => setInvoice(data))
            .catch((error) => console.error("Error fetching invoice data:", error));
    }, [id]);

    const priceWithVat = invoice.price && invoice.vat
        ? Number(invoice.price) * (1 + Number(invoice.vat) / 100)
        : null;

    return (
        <div>
            <Breadcrumb items={[
                { to: "/", label: "Přehled" },
                { to: "/invoices", label: "Faktury" },
                { label: `Faktura č. ${invoice.invoiceNumber || ""}` }
            ]} />
            <h1>Detail faktury</h1>
            <hr />
            <div className="card shadow-sm">
                <div className="card-header bg-primary text-white">
                    <h4 className="mb-0">Faktura č. {invoice.invoiceNumber}</h4>
                </div>
                <div className="card-body">
                    <div className="row">
                        <div className="col-md-6">
                            <p><strong>Vystaveno:</strong> {invoice.issued && dateStringFormatter(invoice.issued)}</p>
                            <p><strong>Splatnost:</strong> {invoice.dueDate && dateStringFormatter(invoice.dueDate)}</p>
                            <p><strong>Produkt:</strong> {invoice.product}</p>
                            <p><strong>Poznámka:</strong> {invoice.note || "—"}</p>
                        </div>
                        <div className="col-md-6">
                            <p><strong>Cena bez DPH:</strong> {invoice.price && formatPrice(invoice.price)}</p>
                            <p><strong>DPH:</strong> {invoice.vat} %</p>
                            {priceWithVat && (
                                <p className="fs-5"><strong>Cena s DPH:</strong> <span className="text-primary fw-bold">{formatPrice(priceWithVat)}</span></p>
                            )}
                        </div>
                    </div>
                    <hr />
                    <div className="row">
                        <div className="col-md-6">
                            <h6>Prodejce</h6>
                            {invoice.seller && (
                                <Link to={`/persons/show/${invoice.seller._id}`} className="text-decoration-none">
                                    {invoice.seller.name}
                                </Link>
                            )}
                        </div>
                        <div className="col-md-6">
                            <h6>Kupující</h6>
                            {invoice.buyer && (
                                <Link to={`/persons/show/${invoice.buyer._id}`} className="text-decoration-none">
                                    {invoice.buyer.name}
                                </Link>
                            )}
                        </div>
                    </div>
                </div>
                <div className="card-footer d-flex gap-2">
                    <Link to={`/invoices/edit/${id}`} className="btn btn-warning btn-sm">Upravit</Link>
                    <Link to="/invoices" className="btn btn-outline-secondary btn-sm">Zpět na seznam</Link>
                </div>
            </div>
        </div>
    );
};

export default InvoiceDetail;
