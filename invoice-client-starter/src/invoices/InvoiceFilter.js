import React, { useState, useEffect } from "react";
import { apiGet } from "../utils/api";
import InputField from "../components/InputField";
import InputSelect from "../components/InputSelect";

/**
 * Component for filtering invoices by various criteria.
 * 
 * @param {Object} props - Component properties
 * @param {Function} props.onFilter - Function to call when filter is applied
 * @param {Function} props.onReset - Function to call when filter is reset
 */
const InvoiceFilter = ({ onFilter, onReset }) => {
    const [persons, setPersons] = useState([]);
    const [filter, setFilter] = useState({
        invoiceNumber: "",
        product: "",
        minPrice: "",
        maxPrice: "",
        issuedFrom: "",
        issuedTo: "",
        sellerId: "",
        buyerId: "",
        limit: ""
    });
    const [loading, setLoading] = useState(true);

    const [products, setProducts] = useState([]);

    useEffect(() => {
        setLoading(true);
        Promise.all([
            apiGet("/api/persons"),
            apiGet("/api/invoices")
        ])
            .then(([personData, invoiceData]) => {
                setPersons(personData || []);
                const unique = [...new Set((invoiceData || []).map(i => i.product).filter(Boolean))].sort();
                setProducts(unique);
            })
            .catch(error => {
                console.error("Chyba při načítání dat:", error);
            })
            .finally(() => {
                setLoading(false);
            });
    }, []);

    // Obsluha změn filtru
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFilter({
            ...filter,
            [name]: value
        });
    };

    // Odeslání filtru
    const handleSubmit = (e) => {
        e.preventDefault();
        
        // Předáme data dál bez dalších úprav
        // Konverze a vyčištění se provede v API funkci
        onFilter(filter);
    };

    // Reset filtru
    const handleReset = () => {
        setFilter({
            invoiceNumber: "",
            product: "",
            minPrice: "",
            maxPrice: "",
            issuedFrom: "",
            issuedTo: "",
            sellerId: "",
            buyerId: "",
            limit: ""
        });
        onReset();
    };

    // Kontrola, zda je aspoň jedno filtrační kritérium vyplněné
    const hasFilter = Object.values(filter).some(value => value !== "");

    return (
        <div className="card mb-4">
            <div className="card-header bg-light">
                <h5 className="mb-0">Filtrování faktur</h5>
            </div>
            <div className="card-body">
                {loading ? (
                    <div className="text-center p-3">
                        <div className="spinner-border spinner-border-sm" role="status">
                            <span className="visually-hidden">Načítání...</span>
                        </div>
                        <span className="ms-2">Načítání dat...</span>
                    </div>
                ) : (
                    <form onSubmit={handleSubmit}>
                        <div className="row">
                            <div className="col-md-4">
                                <InputField
                                    type="number"
                                    name="invoiceNumber"
                                    label="Číslo faktury"
                                    value={filter.invoiceNumber}
                                    handleChange={handleChange}
                                />
                            </div>
                            <div className="col-md-4">
                                <div className="form-group">
                                    <label>Produkt:</label>
                                    <select
                                        className="form-select"
                                        name="product"
                                        value={filter.product}
                                        onChange={handleChange}
                                    >
                                        <option value="">(Všechny produkty)</option>
                                        {products.map((p, i) => (
                                            <option key={i} value={p}>{p}</option>
                                        ))}
                                    </select>
                                </div>
                            </div>
                            <div className="col-md-4">
                                <InputField
                                    type="number"
                                    name="limit"
                                    label="Limit počtu výsledků"
                                    value={filter.limit}
                                    handleChange={handleChange}
                                />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-md-3">
                                <InputField
                                    type="number"
                                    name="minPrice"
                                    label="Minimální cena"
                                    value={filter.minPrice}
                                    handleChange={handleChange}
                                />
                            </div>
                            <div className="col-md-3">
                                <InputField
                                    type="number"
                                    name="maxPrice"
                                    label="Maximální cena"
                                    value={filter.maxPrice}
                                    handleChange={handleChange}
                                />
                            </div>
                            <div className="col-md-3">
                                <InputField
                                    type="date"
                                    name="issuedFrom"
                                    label="Vystaveno od"
                                    value={filter.issuedFrom}
                                    handleChange={handleChange}
                                />
                            </div>
                            <div className="col-md-3">
                                <InputField
                                    type="date"
                                    name="issuedTo"
                                    label="Vystaveno do"
                                    value={filter.issuedTo}
                                    handleChange={handleChange}
                                />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-md-6">
                                <InputSelect
                                    name="sellerId"
                                    label="Prodejce"
                                    prompt="Vyberte prodejce"
                                    items={persons}
                                    value={filter.sellerId}
                                    handleChange={handleChange}
                                />
                            </div>
                            <div className="col-md-6">
                                <InputSelect
                                    name="buyerId"
                                    label="Kupující"
                                    prompt="Vyberte kupujícího"
                                    items={persons}
                                    value={filter.buyerId}
                                    handleChange={handleChange}
                                />
                            </div>
                        </div>
                        <div className="mt-3">
                            <button 
                                type="submit" 
                                className="btn btn-primary me-2"
                                disabled={!hasFilter}  // Zakáže tlačítko, pokud není nic vyplněno
                            >
                                Filtrovat
                            </button>
                            <button 
                                type="button" 
                                className="btn btn-secondary" 
                                onClick={handleReset}
                                disabled={!hasFilter}  // Zakáže tlačítko, pokud není nic vyplněno
                            >
                                Zrušit filtr
                            </button>
                        </div>
                        {!hasFilter && (
                            <div className="alert alert-info mt-3">
                                <small>Pro filtrování vyplňte alespoň jedno kritérium.</small>
                            </div>
                        )}
                    </form>
                )}
            </div>
        </div>
    );
};

export default InvoiceFilter;