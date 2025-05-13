import React from "react";

/**
 * Component for text, number, date input elements and textareas.
 * 
 * @param {Object} props - Component properties
 * @param {string} props.type - Input type ("text", "number", "date", "textarea")
 * @param {string} props.name - Input name attribute
 * @param {string} props.value - Input value
 * @param {string} props.label - Label text for the input
 * @param {string} props.prompt - Placeholder text
 * @param {boolean} props.required - Whether the input is required
 * @param {number} props.min - Minimum value (for number/date) or minimum length (for text/textarea)
 * @param {number} props.rows - Number of rows for textarea
 * @param {Function} props.handleChange - Change event handler
 */
export function InputField(props) {
  // Supported input types
  const INPUTS = ["text", "number", "date"];

  // Element and type validation
  const type = props.type.toLowerCase();
  const isTextarea = type === "textarea";
  const required = props.required || false;

  if (!isTextarea && !INPUTS.includes(type)) {
    return null;
  }

  // Assign minimum value to the appropriate attribute based on type
  const minProp = props.min || null;
  const min = ["number", "date"].includes(type) ? minProp : null;
  const minlength = ["text", "textarea"].includes(type) ? minProp : null;

  return (
    <div className="form-group">
      <label>{props.label}:</label>

      {/* Render the appropriate element */}
      {isTextarea ? (
        <textarea
          required={required}
          className="form-control"
          placeholder={props.prompt}
          rows={props.rows}
          minLength={minlength}
          name={props.name}
          value={props.value}
          onChange={props.handleChange}
        />
      ) : (
        <input
          required={required}
          type={type}
          className="form-control"
          placeholder={props.prompt}
          minLength={minlength}
          min={min}
          name={props.name}
          value={props.value}
          onChange={props.handleChange}
        />
      )}
    </div>
  );
}

export default InputField;