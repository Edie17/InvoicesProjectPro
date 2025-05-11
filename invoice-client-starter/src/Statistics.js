import React, { useState, useEffect } from "react";
import { apiGet } from "./utils/api";
import { Link } from "react-router-dom";

const Statistics = () => {
  const [invoiceStats, setInvoiceStats] = useState({
    currentYearSum: 0,
    allTimeSum: 0,
    invoicesCount: 0
  });
  const [personStats, setPersonStats] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    setLoading(true);
    setError(null);

    // Načtení dat z oficiálních endpointů podle dokumentace
    Promise.all([
      apiGet("/api/invoices/statistics"),
      apiGet("/api/persons/statistics")
    ])
      .then(([invoiceData, personData]) => {
        console.log("Statistiky faktur:", invoiceData);
        console.log("Statistiky osob:", personData);
        
        // Nastavení dat faktur podle formátu z dokumentace
        setInvoiceStats({
          currentYearSum: invoiceData.currentYearSum || 0,
          allTimeSum: invoiceData.allTimeSum || 0,
          invoicesCount: invoiceData.invoicesCount || 0
        });
        
        // Nastavení dat osob
        setPersonStats(Array.isArray(personData) ? personData : []);
        setLoading(false);
      })
      .catch(err => {
        console.error("Chyba při načítání statistik:", err);
        setError("Nastala chyba při načítání statistik. Zkuste to prosím později.");
        setLoading(false);
      });
  }, []);

  // Pomocná funkce pro formátování čísel
  const formatNumber = (num) => {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
  };

  if (loading) return (
    <div className="text-center mt-5">
      <div className="spinner-border" role="status">
        <span className="visually-hidden">Načítání...</span>
      </div>
      <h3 className="mt-2">Načítám statistiky...</h3>
    </div>
  );
  
  if (error) return (
    <div className="alert alert-danger" role="alert">
      <h4 className="alert-heading">Chyba!</h4>
      <p>{error}</p>
    </div>
  );

  return (
    <div>
      <h1>Fakturační systém</h1>
      <p className="lead">Vítejte v aplikaci pro správu faktur a osob.</p>
      <hr />
      
      <div className="row mb-5">
        <div className="col-md-12">
          <div className="card">
            <div className="card-header bg-primary text-white">
              <h3 className="mb-0">Obecné statistiky faktur</h3>
            </div>
            <div className="card-body">
              <div className="row">
                <div className="col-md-4">
                  <div className="card bg-light">
                    <div className="card-body text-center">
                      <h5 className="card-title">Příjmy v letošním roce</h5>
                      <h2 className="text-primary">{formatNumber(invoiceStats.currentYearSum)} Kč</h2>
                    </div>
                  </div>
                </div>
                <div className="col-md-4">
                  <div className="card bg-light">
                    <div className="card-body text-center">
                      <h5 className="card-title">Celkové příjmy</h5>
                      <h2 className="text-primary">{formatNumber(invoiceStats.allTimeSum)} Kč</h2>
                    </div>
                  </div>
                </div>
                <div className="col-md-4">
                  <div className="card bg-light">
                    <div className="card-body text-center">
                      <h5 className="card-title">Počet faktur</h5>
                      <h2 className="text-primary">{invoiceStats.invoicesCount}</h2>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      <div className="row mb-4">
        <div className="col-md-12">
          <div className="card">
            <div className="card-header bg-success text-white">
              <h3 className="mb-0">Příjmy podle společností</h3>
            </div>
            <div className="card-body">
              <table className="table table-striped">
                <thead>
                  <tr>
                    <th>Společnost / Osoba</th>
                    <th>Fakturované příjmy</th>
                    <th>Akce</th>
                  </tr>
                </thead>
                <tbody>
                  {personStats.length > 0 ? (
                    personStats.map((person) => (
                      <tr key={person.personId}>
                        <td>{person.personName}</td>
                        <td>{formatNumber(person.revenue)} Kč</td>
                        <td>
                          <Link 
                            to={`/persons/show/${person.personId}`}
                            className="btn btn-sm btn-info"
                          >
                            Zobrazit detail
                          </Link>
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan="3" className="text-center">Žádné statistiky k zobrazení</td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
      
      <div className="row">
        <div className="col-md-6">
          <div className="card">
            <div className="card-header bg-info text-white">
              <h3 className="mb-0">Rychlé odkazy</h3>
            </div>
            <div className="card-body">
              <div className="list-group">
                <Link to="/persons" className="list-group-item list-group-item-action">
                  Správa osob
                </Link>
                <Link to="/persons/create" className="list-group-item list-group-item-action">
                  Vytvořit novou osobu
                </Link>
                <Link to="/invoices" className="list-group-item list-group-item-action">
                  Správa faktur
                </Link>
                <Link to="/invoices/create" className="list-group-item list-group-item-action">
                  Vytvořit novou fakturu
                </Link>
              </div>
            </div>
          </div>
        </div>
        <div className="col-md-6">
          <div className="card">
            <div className="card-header bg-warning text-dark">
              <h3 className="mb-0">O aplikaci</h3>
            </div>
            <div className="card-body">
              <p>
                Tato aplikace slouží ke správě faktur a osob. Můžete zde vytvářet, upravovat a mazat faktury 
                a osoby, a také sledovat statistiky příjmů.
              </p>
              <p>
                Pro správu osob přejděte do sekce <strong>Osoby</strong>, pro správu faktur přejděte do sekce <strong>Faktury</strong>.
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Statistics;