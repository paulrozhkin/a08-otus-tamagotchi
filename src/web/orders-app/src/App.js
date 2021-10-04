import logo from './logo.svg';
import './App.css';
import Restaurants from "./components/Restaurants"

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Hello, Orders world!!!
          Edit <code>src/App.js</code> and save to reload.
        </p>

        <Restaurants />

        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
