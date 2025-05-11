import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { apiGet, apiPost, apiPut } from "../utils/api";
import InputField from "../components/InputField";
import InputSelect from "../components/InputSelect";
import FlashMessage from "../components/FlashMessage";

const InvoiceForm = () => {
    const navigate = useNavigate();
    const { id } = useParams();
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
    const [sentState, setSent] = useState(false);
    const [successState, setSuccess] = useState(false);
    const [errorState, setError] = useState(null);

    useEffect(() => {
        // Načtení všech osob pro výběr
        apiGet("/api/persons").then((data) => setPersons(data));

        // Pokud je ID, načteme fakturu pro úpravu
        if (id) {
            apiGet("/api/invoices/" + id).then((data) => {
                const processedData = {
                    ...data,
                    sellerId: data.seller?._id || "",
                    buyerId: data.buyer?._id || "",
                    issued: data.issued ? data.issued.split("T")[0] : "",
                    dueDate: data.dueDate ? data.dueDate.split("T")[0] : ""
                };
                setInvoice(processedData);
            });
        }
    }, [id]);

    const handleSubmit = (e) => {
        e.preventDefault();

        // Připravíme data pro odeslání
        const invoiceData = {
            ...invoice,
            price: parseFloat(invoice.price),
            vat: parseInt(invoice.vat)
        };

        (id ? apiPut("/api/invoices/" + id, invoiceData) : apiPost("/api/invoices", invoiceData))
            .then((data) => {
                setSent(true);
                setSuccess(true);
                navigate("/invoices");
            })
            .catch((error) => {
                console.log(error.message);
                setError(error.message);
                setSent(true);
                setSuccess(false);
            });
    };

    const sent = sentState;
    const success = successState;

    return (
        <div>
            <h1>{id ? "Upravit" : "Vytvořit"} fakturu</h1>
            <hr />
            {errorState ? <div className="alert alert-danger">{errorState}</div> : null}
            {sent && (
                <FlashMessage
                    theme={success ? "success" : ""}
                    text={success ? "Uložení faktury proběhlo úspěšně." : ""}
                />
            )}
            <form onSubmit={handleSubmit}>
                <InputField
                    required={true}
                    type="number"
                    name="invoiceNumber"
                    label="Číslo faktury"
                    prompt="Zadejte číslo faktury"
                    value={invoice.invoiceNumber}
                    handleChange={(e) => {
                        setInvoice({ ...invoice, invoiceNumber: e.target.value });
                    }}
                />

                <InputField
                    required={true}
                    type="date"
                    name="issued"
                    label="Datum vystavení"
                    value={invoice.issued}
                    handleChange={(e) => {
                        setInvoice({ ...invoice, issued: e.target.value });
                    }}
                />

                <InputField
                    required={true}
                    type="date"
                    name="dueDate"
                    label="Datum splatnosti"
                    value={invoice.dueDate}
                    handleChange={(e) => {
                        setInvoice({ ...invoice, dueDate: e.target.value });
                    }}
                />

                <InputField
                    required={true}
                    type="text"
                    name="product"
                    min="3"
                    label="Produkt"
                    prompt="Zadejte název produktu"
                    value={invoice.product}
                    handleChange={(e) => {
                        setInvoice({ ...invoice, product: e.target.value });
                    }}
                />

                <InputField
                    required={true}
                    type="number"
                    name="price"
                    label="Cena"
                    prompt="Zadejte cenu"
                    value={invoice.price}
                    handleChange={(e) => {
                        setInvoice({ ...invoice, price: e.target.value });
                    }}
                />

                <InputField
                    required={true}
                    type="number"
                    name="vat"
                    label="DPH (%)"
                    prompt="Zadejte sazbu DPH"
                    value={invoice.vat}
                    handleChange={(e) => {
                        setInvoice({ ...invoice, vat: e.target.value });
                    }}
                />

                <InputSelect
                    required={true}
                    name="sellerId"
                    label="Prodejce"
                    prompt="Vyberte prodejce"
                    items={persons}
                    value={invoice.sellerId}
                    handleChange={(e) => {
                        setInvoice({ ...invoice, sellerId: e.target.value });
                    }}
                />

                <InputSelect
                    required={true}
                    name="buyerId"
                    label="Kupující"
                    prompt="Vyberte kupujícího"
                    items={persons}
                    value={invoice.buyerId}
                    handleChange={(e) => {
                        setInvoice({ ...invoice, buyerId: e.target.value });
                    }}
                />

                <InputField
                    type="textarea"
                    name="note"
                    label="Poznámka"
                    rows="5"
                    value={invoice.note}
                    handleChange={(e) => {
                        setInvoice({ ...invoice, note: e.target.value });
                    }}
                />

                <input type="submit" className="btn btn-primary" value="Uložit" />
            </form>
        </div>
    );
};

export default InvoiceForm;