import "./styles.css";
import React, { useState, useEffect, useCallback } from "react";

function Calculator({ accessToken }) {
  const [firstValue, setFirstValue] = useState("");
  const [secondValue, setSecondValue] = useState("");
  const [operation, setOperation] = useState("");
  const [result, setResult] = useState(null);

  const handleOperationClick = useCallback(async () => {
    console.log("Valores enviados para a API:", {
      firstValue,
      secondValue,
      operation,
    });

    try {
      const response = await fetch(
        "http://localhost:8080/api/Calculator/calculate",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
          },
          body: JSON.stringify({ firstValue, secondValue, operation }),
        }
      );

      if (response.ok) {
        const data = await response.json();
        const resultValue = data.result;

        setResult(resultValue);
        window.alert(`Operation Result: ${resultValue}`);
      } else {
        const errorData = await response.json();
        console.error("Operation error, please try again:", errorData.message);
        window.alert(`Operation error, please try again`);
      }
    } catch (error) {
      console.error("Error processing request:", error);
      window.alert(`Error processing request`);
    } finally {
      setFirstValue("");
      setSecondValue("");
      setOperation("");
    }
  }, [accessToken, firstValue, secondValue, operation]);

  useEffect(() => {
    if (operation !== "") {
      handleOperationClick();
    }
  }, [operation, handleOperationClick]);

  const handleButtonClick = (op) => {
    setOperation(op);
  };

  return (
    <div className="container">
      <div className="container-login">
        <div className="wrap-login">
          <form className="login-form">
            <span className="login-form-title">Calculator</span>

            <div className="wrap-input">
              <input
                className={firstValue !== "" ? "has-val input" : "input"}
                value={firstValue}
                type="text"
                onChange={(e) => setFirstValue(e.target.value)}
              />
              <span
                className="focus-input"
                data-placeholder="First Value"
              ></span>
            </div>

            <div className="wrap-input">
              <input
                className={secondValue !== "" ? "has-val input" : "input"}
                value={secondValue}
                type="text"
                onChange={(e) => setSecondValue(e.target.value)}
              />
              <span
                className="focus-input"
                data-placeholder="Second Value"
              ></span>
            </div>

            <div className="container-calculator-form-btn">
              <button
                type="button"
                className="calculator-form-btn"
                onClick={() => handleButtonClick("sum")}
              >
                Sum
              </button>
              <button
                type="button"
                className="calculator-form-btn"
                onClick={() => handleButtonClick("subtraction")}
              >
                Subtraction
              </button>
              <button
                type="button"
                className="calculator-form-btn"
                onClick={() => handleButtonClick("multiplication")}
              >
                Multiplication
              </button>
              <button
                type="button"
                className="calculator-form-btn"
                onClick={() => handleButtonClick("division")}
              >
                Division
              </button>
            </div>
          </form>
        </div>
      </div>
      {result !== null && <p>Resultado: {result}</p>}
    </div>
  );
}

export default Calculator;
