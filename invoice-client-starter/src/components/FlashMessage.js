import React from "react";

/**
 * Component for displaying alert/notification messages to the user.
 * 
 * @param {Object} props - Component properties
 * @param {string} props.theme - Bootstrap theme for the alert (e.g. "success", "danger", "warning")
 * @param {string} props.text - Message text to display
 */
export function FlashMessage({ theme, text }) {
  return <div className={"alert alert-" + theme}>{text}</div>;
}

export default FlashMessage;