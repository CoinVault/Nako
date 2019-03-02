import React, { Component } from 'react';
import './style.css';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom'

import { Grid } from 'react-bootstrap';
//import { Grid, Table, Row, Col } from 'react-bootstrap';

class Address extends Component {

  constructor(props) {
    super(props);

    this.state = {
      block: { transactions : []},
      nextBlock: { transactions : []},
      transactions: [],
      address:{ address: 'Loading...', transactions : []}
    };
  }

  componentDidMount() {
    
    let address = this.props.match.params.address;
   
   
    fetch(`/api/query/address/${address}/transactions`,{mode: 'cors'})
            .then(result=>result.json())
            .then(address=>this.setState({address}));
  }

  componentWillReceiveProps(nextProps) {
    this.props = nextProps;
    
  }
  
  render() {
    return (
      <Grid>
        <div className="Block">
          <div className="">
              <img src='/nako_logo.png' width="60" />
          </div>
          <div className="well">
              <h1>{this.state.block.coinTag} Block explorer</h1>
              <Link to="/">Home</Link>
          </div>
               
          

          <h2>{this.state.address.address}</h2>
          <table className="table table-striped">
            <thead>
              <tr>
                <th></th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>Balance</td>
                <td>{this.state.address.balance}</td>
              </tr>
              <tr>
                <td>Unconfirmed</td>
                <td>{this.state.address.unconfirmedBalance}</td>
              </tr>
              <tr>
                <td>Total Received</td>
                <td>{this.state.address.totalReceived}</td>
              </tr>
              <tr>
                <td>Total Sent</td>
                <td>{this.state.address.totalSent}</td>
              </tr>
            </tbody>
          
          </table>

          <h3>Raw</h3>
          <pre>{JSON.stringify(this.state.address)}</pre>

          <br/>
          <br/>
          <h3>Transactions</h3>
          <table className="table table-striped">
            <thead>
              <tr>
                <th>Transaction Id</th>
                <th>Value</th> 
              </tr>
            </thead>
            <tbody>
              
            {this.state.address.transactions
              .map(function(object, i){
                  return <tr key={i}>
                      <td><Link to={"/transaction/" +  (object.transactionHash) }> {object.transactionHash }</Link></td>
                      <td>{object.value}</td>
                  </tr>
                })}
          </tbody>
          </table>
        </div>
      </Grid>
    );
  }
}
export default Address;