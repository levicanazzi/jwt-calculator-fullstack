import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import threeoImg from "../assets/threeo.png";

function Login({ setAccessToken }) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch("http://localhost:8080/api/auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ username, password }),
      });

      if (response.ok) {
        const data = await response.json();
        const token = data.accessToken;

        setAccessToken(token);

        console.log("Access Token:", token);

        setTimeout(() => {
          window.alert("Authorized Login");
        }, 100);

        navigate("/calculator");
      } else {
        const errorData = await response.json();
        console.error("Authentication Error:", errorData.message);
        window.alert(`Authentication Error, please try again`);
      }
    } catch (error) {
      console.error("Error processing request:", error);
      window.alert(`Error processing request`);
    }
  };

  return (
    <div className="container">
      <div className="container-login">
        <div className="wrap-login">
          <form className="login-form" onSubmit={handleLogin}>
            <span className="login-form-title">
              <img src={threeoImg} alt="Threeo Login" />
            </span>

            <div className="wrap-input">
              <input
                className={username !== "" ? "has-val input" : "input"}
                type="username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
              <span className="focus-input" data-placeholder="Username"></span>
            </div>

            <div className="wrap-input">
              <input
                className={password !== "" ? "has-val input" : "input"}
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
              <span className="focus-input" data-placeholder="Password"></span>
            </div>

            <div className="container-login-form-btn">
              <button className="login-form-btn" type="submit">
                Login
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}

export default Login;
