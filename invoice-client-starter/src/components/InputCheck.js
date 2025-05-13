/**
 * Component for checkbox and radio input elements.
 * 
 * @param {Object} props - Component properties
 * @param {string} props.type - Input type ("checkbox" or "radio")
 * @param {string} props.name - Input name attribute
 * @param {string} props.value - Input value
 * @param {boolean} props.checked - Whether the input is checked
 * @param {string} props.label - Label text for the input
 * @param {Function} props.handleChange - Change event handler
 */
export function InputCheck(props) {
  // Supported input types
  const INPUTS = ["checkbox", "radio"];

  // Type validation
  const type = props.type.toLowerCase();
  const checked = props.checked || "";

  if (!INPUTS.includes(type)) {
    return null;
  }

  return (
    <div className="form-group form-check">
      <label className="form-check-label">
        {/* Render with current type */}
        <input
          type={props.type}
          className="form-check-input"
          name={props.name}
          value={props.value}
          checked={checked}
          onChange={props.handleChange}
        />{" "}
        {props.label}
      </label>
    </div>
  );
}

export default InputCheck;