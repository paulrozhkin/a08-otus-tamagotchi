import './App.css';
import Layout from "./components/Layout"
import React, { Component } from 'react';
import {BrowserRouter as Router} from "react-router-dom";
function App() {
  return (
    <Router>
      <Layout />
    </Router>
  );
}

export default App;
