import React from "react";

export default function FormField({
  label,
  type = "text",
  name,
  value,
  onChange,
  onBlur,
  error,
  required = false,
  inputProps = {},
}) {
  return (
    <label className="field">
      <span className="label-text">
        {label}
        {required ? " *" : ""}
      </span>
      <input
        type={type}
        name={name}
        value={value}
        onChange={onChange}
        onBlur={onBlur}
        required={required}
        className={error ? "invalid" : ""}
        aria-invalid={error ? "true" : "false"}
        {...inputProps}
      />
      {error && <div className="field-error">{error}</div>}
    </label>
  );
}
