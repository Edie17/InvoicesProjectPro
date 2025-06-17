import React from "react";

/**
 * Component for select dropdown elements, supporting both single and multiple selection.
 * 
 * @param {Object} props - Component properties
 * @param {string} props.name - Select name attribute
 * @param {string} props.label - Label text for the select
 * @param {string} props.prompt - Placeholder text for empty option
 * @param {boolean} props.multiple - Whether multiple selection is allowed
 * @param {boolean} props.required - Whether selection is required
 * @param {Array} props.items - Array of items to populate the dropdown
 * @param {Object} props.enum - Optional object mapping item values to display names
 * @param {string|Array} props.value - Selected value(s)
 * @param {Function} props.handleChange - Change event handler
 */
export function InputSelect(props) {
  const multiple = props.multiple;
  const required = props.required || false;

  // Flag for empty value selection
  const emptySelected = multiple ? props.value?.length === 0 : !props.value;
  // Flag for object-structured items
  const objectItems = props.enum ? false : true;

  return (
    <div className="form-group">
      <label>{props.label}:</label>
      <select
        required={required}
        className="browser-default form-select"
        multiple={multiple}
        name={props.name}
        onChange={props.handleChange}
        value={props.value}
      >
        {required ? (
          /* Empty option not allowed (for record editing) */
          <option disabled value={emptySelected}>
            {props.prompt}
          </option>
        ) : (
          /* Empty option allowed (for overview filtering) */
          <option key={0} value="">
            ({props.prompt})
          </option>
        )}

        {objectItems
          ? /* Render items as objects from database (persons) */
            props.items.map((item, index) => (
              <option key={required ? index : index + 1} value={item._id}>
                {item.name}
              </option>
            ))
          : /* Render items as values from enumeration (genres) */
            props.items.map((item, index) => (
              <option key={required ? index : index + 1} value={item}>
                {props.enum[item]}
              </option>
            ))}
      </select>
    </div>
  );
}

export default InputSelect;