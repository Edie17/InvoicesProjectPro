import React, { useEffect, useState } from "react";
import { apiDelete, apiGet } from "../utils/api";
import { useToast } from "../components/ToastContext";
import InvoiceTable from "./InvoiceTable";

const InvoiceIndex = () => {
    const [invoices, setInvoices] = useState([]);
    const showToast = useToast();

    const deleteInvoice = async (id) => {
        try {
            await apiDelete("/api/invoices/" + id);
            setInvoices(invoices.filter((item) => item._id !== id));
            showToast("Faktura byla smazána.", "success");
        } catch (error) {
            showToast(error.message, "danger");
        }
    };

    useEffect(() => {
        apiGet("/api/invoices").then((data) => setInvoices(data));
    }, []);

    return (
        <div>
            <h1>Seznam faktur</h1>
            <InvoiceTable deleteInvoice={deleteInvoice} items={invoices} label="Počet faktur:" />
        </div>
    );
};

export default InvoiceIndex;
