import React, { Component } from 'react';
//import logo from './logo.svg';
import './style.css';
import Block from './../Block';
import Home from './../Home';
import Transaction from './../Transaction';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom'

class App extends Component {
  render() {
    return (
      <Router>
        
      <div className="App">

        
        <div className="App-nav">
        
            <div>
            
            {/* <Link to="/block">Block</Link> */}
            <Route exact path="/" component={Home}/>
            {/* <Route exact path="/block" render={() => (
              <h3>Please select a blockHash.</h3>
            )}/> */}
            <Route path="/block/:blockIndex" component={Block}/>
            <Route path="/transaction/:transactionId" component={Transaction}/>
           
            </div>
		      
        </div>
        
      </div>
      </Router>
    );
  }
}
export default App;