import React, { useEffect, useState } from "react";
import { apiDelete, apiGet } from "../utils/api";
import { useToast } from "../components/ToastContext";
import PersonTable from "./PersonTable";

const PersonIndex = () => {
    const [persons, setPersons] = useState([]);
    const showToast = useToast();

    const deletePerson = async (id) => {
        try {
            await apiDelete("/api/persons/" + id);
            setPersons(persons.filter((item) => item._id !== id));
            showToast("Osoba byla smazána.", "success");
        } catch (error) {
            showToast(error.message, "danger");
        }
    };

    useEffect(() => {
        apiGet("/api/persons").then((data) => setPersons(data));
    }, []);

    return (
        <div>
            <h1>Seznam osob</h1>
            <PersonTable deletePerson={deletePerson} items={persons} label="Počet osob:" />
        </div>
    );
};

export default PersonIndex;
