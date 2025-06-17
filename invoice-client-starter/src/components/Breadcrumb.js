import React from "react";
import { Link } from "react-router-dom";

const Breadcrumb = ({ items }) => (
    <nav aria-label="breadcrumb" className="mb-3">
        <ol className="breadcrumb">
            {items.map((item, i) =>
                i < items.length - 1 ? (
                    <li key={i} className="breadcrumb-item">
                        <Link to={item.to}>{item.label}</Link>
                    </li>
                ) : (
                    <li key={i} className="breadcrumb-item active" aria-current="page">
                        {item.label}
                    </li>
                )
            )}
        </ol>
    </nav>
);

export default Breadcrumb;
