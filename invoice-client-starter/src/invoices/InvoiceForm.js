import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { apiGet, apiPost, apiPut } from "../utils/api";
import { useToast } from "../components/ToastContext";
import Breadcrumb from "../components/Breadcrumb";
import InputField from "../components/InputField";
import InputSelect from "../components/InputSelect";
import formatPrice from "../utils/formatPrice";

const InvoiceForm = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const showToast = useToast();
    const [persons, setPersons] = useState([]);
    const [invoice, setInvoice] = useState({
        invoiceNumber: "",
        issued: new Date().toISOString().split("T")[0],
        dueDate: new Date(Date.now() + 14 * 24 * 60 * 60 * 1000).toISOString().split("T")[0],
        product: "",
        price: 0,
        vat: 21,
        note: "",
        sellerId: "",
        buyerId: ""
    });

    useEffect(() => {
        apiGet("/api/persons").then((data) => setPersons(data));
        if (id) {
            apiGet("/api/invoices/" + id).then((data) => {
                setInvoice({
                    ...data,
                    sellerId: data.seller?._id || "",
                    buyerId: data.buyer?._id || "",
                    issued: data.issued ? data.issued.split("T")[0] : "",
                    dueDate: data.dueDate ? data.dueDate.split("T")[0] : ""
                });
            });
        }
    }, [id]);

    const priceWithVat = invoice.price && invoice.vat
        ? Number(invoice.price) * (1 + Number(invoice.vat) / 100)
        : null;

    const handleSubmit = (e) => {
        e.preventDefault();
        const invoiceData = {
            ...invoice,
            price: parseFloat(invoice.price),
            vat: parseInt(invoice.vat)
        };
        (id ? apiPut("/api/invoices/" + id, invoiceData) : apiPost("/api/invoices", invoiceData))
            .then(() => {
                showToast(id ? "Faktura byla upravena." : "Faktura byla vytvořena.", "success");
                navigate("/invoices");
            })
            .catch((error) => {
                showToast(error.message, "danger");
            });
    };

    return (
        <div>
            <Breadcrumb items={[
                { to: "/", label: "Přehled" },
                { to: "/invoices", label: "Faktury" },
                { label: id ? "Upravit fakturu" : "Nová faktura" }
            ]} />
            <h1>{id ? "Upravit" : "Vytvořit"} fakturu</h1>
            <hr />
            <form onSubmit={handleSubmit}>
                <InputField
                    required={true} type="number" name="invoiceNumber" label="Číslo faktury"
                    prompt="Zadejte číslo faktury" value={invoice.invoiceNumber}
                    handleChange={(e) => setInvoice({ ...invoice, invoiceNumber: e.target.value })}
                />
                <InputField
                    required={true} type="date" name="issued" label="Datum vystavení"
                    value={invoice.issued}
                    handleChange={(e) => setInvoice({ ...invoice, issued: e.target.value })}
                />
                <InputField
                    required={true} type="date" name="dueDate" label="Datum splatnosti"
                    value={invoice.dueDate}
                    handleChange={(e) => setInvoice({ ...invoice, dueDate: e.target.value })}
                />
                <InputField
                    required={true} type="text" name="product" min="3" label="Produkt"
                    prompt="Zadejte název produktu" value={invoice.product}
                    handleChange={(e) => setInvoice({ ...invoice, product: e.target.value })}
                />
                <InputField
                    required={true} type="number" name="price" label="Cena bez DPH (Kč)"
                    prompt="Zadejte cenu" value={invoice.price}
                    handleChange={(e) => setInvoice({ ...invoice, price: e.target.value })}
                />
                <InputField
                    required={true} type="number" name="vat" label="DPH (%)"
                    prompt="Zadejte sazbu DPH" value={invoice.vat}
                    handleChange={(e) => setInvoice({ ...invoice, vat: e.target.value })}
                />
                {priceWithVat !== null && (
                    <div className="alert alert-info py-2 mb-3">
                        Cena s DPH ({invoice.vat} %): <strong>{formatPrice(priceWithVat)}</strong>
                    </div>
                )}
                <InputSelect
                    required={true} name="sellerId" label="Prodejce" prompt="Vyberte prodejce"
                    items={persons} value={invoice.sellerId}
                    handleChange={(e) => setInvoice({ ...invoice, sellerId: e.target.value })}
                />
                <InputSelect
                    required={true} name="buyerId" label="Kupující" prompt="Vyberte kupujícího"
                    items={persons} value={invoice.buyerId}
                    handleChange={(e) => setInvoice({ ...invoice, buyerId: e.target.value })}
                />
                <InputField
                    type="textarea" name="note" label="Poznámka" rows="5" value={invoice.note}
                    handleChange={(e) => setInvoice({ ...invoice, note: e.target.value })}
                />
                <input type="submit" className="btn btn-primary" value="Uložit" />
            </form>
        </div>
    );
};

export default InvoiceForm;
