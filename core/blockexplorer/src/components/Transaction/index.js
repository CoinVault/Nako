import React, { Component } from 'react';
import './style.css';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom'
import { Table } from 'react-bootstrap';
import { Grid } from 'react-bootstrap';

class Transaction extends Component {
  constructor(props) {
    super(props);

    this.state = {
      transaction: { inputs:[], outputs:[]},
    };
  }

  componentDidMount() {
    let transactionId = this.props.match.params.transactionId;
   
    fetch(`/api/query/transaction/${transactionId}`,{mode: 'cors'})
            .then(result=>result.json())
            .then(transaction=> this.setState({transaction}));
  }
  
  render() {
    return (
      <Grid>
        <div className="Block">
          <div className="jumbotron">
            <h1>{this.state.transaction.coinTag} Block explorer</h1>
          </div>
          <h2>Transaction Id: {this.state.transaction.transactionId}</h2>
         
          <Table>
            <thead>
              <tr>
                <th></th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>Block height</td>
                <td><Link to={"/block/" + this.state.transaction.blockIndex}>{this.state.transaction.blockIndex}</Link></td>
              </tr>
              <tr>
                <td>Timestamp</td>
                <td>{this.state.transaction.timestamp}</td>
              </tr>
              <tr>
                <td>Blockhash</td>
                <td>{this.state.transaction.blockHash}</td>
              </tr>
              <tr>
                <td>Confirmations</td>
                <td>{this.state.transaction.confirmations}</td>
              </tr>
              <tr>
                <td>Inputs</td>
                <td><ul>{this.state.transaction.inputs.map((input, index) => <li>Address:[{input.inputAddress}] Coinbase:[{input.coinBase}] Input Transaction Id:[{input.inputTransactionId}] Input Index:[{input.inputIndex}]</li>)}</ul></td>
              
              </tr>
              <tr>
                <td>Outputs</td>
                <td><ul>{this.state.transaction.outputs.map((output, index) => <li>Address:[{output.address}] amount:[{output.balance}] Output Index:[{output.index}] Output Type:[{output.outputType}]</li>)}</ul></td>
              
              </tr>
            </tbody>
          
          </Table>
        </div>
      </Grid>
    );
  }
}
export default Transaction;