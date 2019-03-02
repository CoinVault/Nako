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
        <div className="row">
          <div className="col-2">
            <img src='/nako_logo.png' width="60" />
          </div>
          <div className="well col-10">
            <h1>{this.state.transaction.coinTag} Block explorer</h1>
          </div>
        </div>
          <h3>Transaction Id: {this.state.transaction.transactionId}</h3>
         
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
              
            </tbody>
          </Table>

          <br/><br/>

          <table className="table table-striped">
            <thead>
              <tr>
                <th>Transaction Id</th>
                <th>Value out</th> 
                <th>From</th> 
                <th>To</th> 
              </tr>
            </thead>
            <tbody>
                
              <tr>
                        <td className="truncate">{this.state.transaction.transactionId }</td>
                        <td>{this.state.transaction.outputs.length > 1 ? this.state.transaction.outputs.reduce(function(a,b){ return {totalBalance: a.balance + b.balance}}).totalBalance : (this.state.transaction.outputs[0] ? this.state.transaction.outputs[0].balance : '')}</td>
                        <td>
                          {this.state.transaction.inputs
                            .map(function(input, j){
                              return <div>[{input.inputIndex}] {input.inputAddress === ''?'Unknown address':input.inputAddress} <i className="pull-right">{input.balance}</i></div>
                            })}
                        </td>
                        <td>
                          {this.state.transaction.outputs
                            .map(function(output, k){
                              return <div>[{output.index}] <Link to={"/address/" +  (output.address) }>{output.address}</Link> <i className="pull-right">{output.balance}</i></div>
                            })}
                        </td>
                    </tr>
            </tbody>
          </table>
        </div>
      </Grid>
    );
  }
}
export default Transaction;