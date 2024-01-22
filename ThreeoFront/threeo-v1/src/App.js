import Login from "./components/Login";
import Calculator from "./components/Calculator";
import React, { useState } from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";

function App() {
  const [accessToken, setAccessToken] = useState(null);

  const renderCalculator = () => {
    if (accessToken !== null) {
      return <Calculator accessToken={accessToken} />;
    } else {
      return <Navigate to="/" />;
    }
  };

  return (
    <div className="App">
      <Router>
        <Routes>
          <Route path="/" element={<Login setAccessToken={setAccessToken} />} />
          <Route path="/calculator" element={<>{renderCalculator()}</>} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;
