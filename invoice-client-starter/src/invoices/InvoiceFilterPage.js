import React, { useState } from "react";
import { Link } from "react-router-dom";
import { apiFilterInvoices } from "../utils/api";
import InvoiceTable from "./InvoiceTable";
import InvoiceFilter from "./InvoiceFilter";

/**
 * Samostatná stránka pro pokročilé vyhledávání a filtrování faktur.
 */
const InvoiceFilterPage = () => {
    const [invoices, setInvoices] = useState([]);
    const [loading, setLoading] = useState(false);
    const [searched, setSearched] = useState(false);
    const [error, setError] = useState(null);

    /**
     * Filtruje faktury podle kritérií pomocí POST metody
     */
    const handleFilter = (filterData) => {
        setLoading(true);
        setError(null);
        console.log("Filtrační data před odesláním:", filterData);
        
        // Očištění dat před odesláním
        const cleanFilterData = {};
        Object.entries(filterData).forEach(([key, value]) => {
            if (value !== null && value !== undefined && value !== "") {
                if (["invoiceNumber", "limit"].includes(key)) {
                    cleanFilterData[key] = parseInt(value);
                } else if (["minPrice", "maxPrice"].includes(key)) {
                    cleanFilterData[key] = parseFloat(value);
                } else if (["sellerId", "buyerId"].includes(key) && value !== "") {
                    cleanFilterData[key] = parseInt(value);
                } else {
                    cleanFilterData[key] = value;
                }
            }
        });
        
        console.log("Očištěná data pro odeslání:", cleanFilterData);
        
        // Volání API s POST metodou
        apiFilterInvoices(cleanFilterData)
            .then((data) => {
                console.log("Přijatá data z API:", data);
                setInvoices(data || []);
                setSearched(true);
            })
            .catch((error) => {
                console.error("Chyba při filtrování faktur:", error);
                setError("Nepodařilo se filtrovat faktury. " + 
                         (error.message || "Zkontrolujte správnost endpointu a parametrů."));
            })
            .finally(() => {
                setLoading(false);
            });
    };

    /**
     * Reset výsledků vyhledávání
     */
    const handleResetFilter = () => {
        setInvoices([]);
        setSearched(false);
        setError(null);
    };

    /**
     * Zavření chybového hlášení
     */
    const closeError = () => {
        setError(null);
    };

    return (
        <div>
            <h1>Vyhledávání faktur</h1>
            <p className="lead">Zde můžete vyhledávat faktury podle různých kritérií.</p>
            
            {error && (
                <div className="alert alert-danger alert-dismissible fade show" role="alert">
                    <strong>Chyba:</strong> {error}
                    <button type="button" className="btn-close" onClick={closeError}></button>
                </div>
            )}
            
            <InvoiceFilter 
                onFilter={handleFilter}
                onReset={handleResetFilter}
            />
            
            {loading ? (
                <div className="text-center mt-4">
                    <div className="spinner-border" role="status">
                        <span className="visually-hidden">Načítání...</span>
                    </div>
                </div>
            ) : (
                <>
                    {searched && !error && (
                        <div className="alert alert-info mb-3">
                            <strong>Výsledky vyhledávání</strong> - Nalezeno {invoices.length} faktur.
                        </div>
                    )}
                    
                    {searched && !error && invoices.length > 0 && (
                        <InvoiceTable
                            items={invoices}
                            label="Nalezené faktury:"
                        />
                    )}
                    
                    {searched && !error && invoices.length === 0 && (
                        <div className="alert alert-warning">
                            <strong>Žádné výsledky</strong> - Vašim kritériím nevyhovuje žádná faktura.
                        </div>
                    )}
                    
                    <div className="mt-4">
                        <Link to="/invoices" className="btn btn-secondary">
                            Zpět na seznam faktur
                        </Link>
                    </div>
                </>
            )}
        </div>
    );
};

export default InvoiceFilterPage;