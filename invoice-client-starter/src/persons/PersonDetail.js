import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { apiGet } from "../utils/api";
import Breadcrumb from "../components/Breadcrumb";
import Country from "./Country";

const PersonDetail = () => {
    const { id } = useParams();
    const [person, setPerson] = useState({});

    useEffect(() => {
        apiGet("/api/persons/" + id)
            .then((data) => setPerson(data))
            .catch((error) => console.error("Chyba při načítání detailu osoby:", error));
    }, [id]);

    const country = Country.CZECHIA === person.country ? "Česká republika" : "Slovensko";

    return (
        <div>
            <Breadcrumb items={[
                { to: "/", label: "Přehled" },
                { to: "/persons", label: "Osoby" },
                { label: person.name || "Detail osoby" }
            ]} />
            <h1>Detail osoby</h1>
            <hr />
            <div className="card shadow-sm">
                <div className="card-header bg-success text-white">
                    <h4 className="mb-0">{person.name} <small className="fs-6 fw-normal">IČO: {person.identificationNumber}</small></h4>
                </div>
                <div className="card-body">
                    <div className="row">
                        <div className="col-md-6">
                            <p><strong>DIČ:</strong> {person.taxNumber}</p>
                            <p><strong>Telefon:</strong> {person.telephone}</p>
                            <p><strong>E-mail:</strong> <a href={`mailto:${person.mail}`}>{person.mail}</a></p>
                            <p><strong>Poznámka:</strong> {person.note || "—"}</p>
                        </div>
                        <div className="col-md-6">
                            <p><strong>Bankovní účet:</strong> {person.accountNumber}/{person.bankCode}</p>
                            <p><strong>IBAN:</strong> {person.iban}</p>
                            <p><strong>Sídlo:</strong> {person.street}, {person.city}, {person.zip}</p>
                            <p><strong>Země:</strong> {country}</p>
                        </div>
                    </div>
                </div>
                <div className="card-footer d-flex gap-2">
                    <Link to={`/persons/edit/${id}`} className="btn btn-warning btn-sm">Upravit</Link>
                    <Link to="/persons" className="btn btn-outline-secondary btn-sm">Zpět na seznam</Link>
                </div>
            </div>
        </div>
    );
};

export default PersonDetail;
