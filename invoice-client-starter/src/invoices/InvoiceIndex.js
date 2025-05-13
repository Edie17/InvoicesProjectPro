import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { apiDelete, apiGet } from "../utils/api";
import InvoiceTable from "./InvoiceTable";

/**
 * Component for displaying a list of all invoices.
 * Provides functionality to view, edit, and delete invoices.
 */
const InvoiceIndex = () => {
    const [invoices, setInvoices] = useState([]);

    /**
     * Deletes an invoice by its ID.
     * 
     * @param {string} id - The ID of the invoice to delete
     */
    const deleteInvoice = async (id) => {
        try {
            await apiDelete("/api/invoices/" + id);
            setInvoices(invoices.filter((item) => item._id !== id));
        } catch (error) {
            console.log(error.message);
            alert(error.message);
        }
    };

    useEffect(() => {
        apiGet("/api/invoices").then((data) => setInvoices(data));
    }, []);

    return (
        <div>
            <h1>Seznam faktur</h1>
            <InvoiceTable
                deleteInvoice={deleteInvoice}
                items={invoices}
                label="Počet faktur:"
            />
        </div>
    );
};

export default InvoiceIndex;