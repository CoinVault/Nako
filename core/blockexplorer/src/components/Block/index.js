import React, { Component } from 'react';
import './style.css';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom'
import Moment from 'react-moment';
import { Grid } from 'react-bootstrap';
//import { Grid, Table, Row, Col } from 'react-bootstrap';
import Transaction from './../Transaction/index';


class Block extends Component {
  xxx = true;

  constructor(props) {
    super(props);

    this.state = {
      block: { transactions : []},
      nextBlock: { transactions : []},
      transactions: []
    };
  }

  componentDidMount() {
    
    let blockIndex = this.props.match.params.blockIndex;
   
   
    

    fetch(`/api/query/block/Index/${blockIndex}/transactions`,{mode: 'cors'})
            .then(result=>result.json())
            .then(block=>this.setState({block}))
            .then(_ => {
                          for (let i = 0; i < this.state.block.transactions.length; i++) {
                            const transactionId = this.state.block.transactions[i];
                            fetch(`/api/query/transaction/${transactionId}`,{mode: 'cors'})
                            .then(result=>result.json())
                            .then(transaction=>{
                                this.setState({transactions: this.state.transactions.concat(transaction)})
                                this.setState({confirmations: transaction.confirmations})
                                this.setState({latestHeight: parseInt(blockIndex) + transaction.confirmations})
                            });
                        }
              
            });
    let nextIndex = parseInt(blockIndex) + 1;

    fetch(`/api/query/block/Index/${nextIndex}`,{mode: 'cors'})
            .then(result=>result.json())
            .then(nextBlock=>{this.setState({nextBlock})});
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
               
          

          <h2>Block Info: {this.state.block.blockIndex}</h2>
          <table className="table table-striped">
            <thead>
              <tr>
                <th></th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>Block height</td>
                <td>{this.state.block.blockIndex} of <a href={"/block/" + this.state.latestHeight}>{this.state.latestHeight}</a></td>
              </tr>
              <tr>
                <td>Timestamp</td>
                <td>
                  
              <Moment unix>{this.state.block.blockTime}</Moment></td>
              </tr>
              <tr>
                <td>Blockhash</td>
                <td>{this.state.block.blockHash}</td>
              </tr>
              <tr>
                <td>Block size</td>
                <td>{this.state.block.blockSize}</td>
              </tr>
              <tr>
                <td>Confirmations</td>
                <td>{this.state.transactions.length > 0? this.state.transactions[0].confirmations: '?'}</td>
              </tr>
              <tr>
                <td>Previous Blockhash</td>
                <td><a href={"/block/" +  (this.state.block.blockIndex-1) }>{this.state.block.previousBlockHash}</a></td>
                {/* <td><Link to={"/block/" +  (this.state.block.blockIndex-1) }>{this.state.block.previousBlockHash}</Link></td> */}
              </tr>
              <tr>
                <td>Next Blockhash</td>
    <td>{this.state.nextBlock.blockHash  && <a href={"/block/" +  (this.state.block.blockIndex+1) }>{this.state.nextBlock.blockHash}</a>}</td>
                {/* <Link to={"/block/" +  (this.state.block.blockIndex+1) }>nextBlockHashWithLink</Link> */}
              </tr>
            </tbody>
          
          </table>

          <br/>

          {/*<ul className="nav nav-tabs">
            <li className="nav-item">
              <a className="nav-link active" href="#">Transactions</a>
            </li>
            <li className="nav-item">
              <a className="nav-link" href="#">Raw</a>
            </li>
           
          </ul>*/}
          
          <br/>
          <h3>Transactions</h3>
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
              
            {this.state.transactions
              .map(function(object, i){
                  return <tr key={i}>
                      <td className="truncate"><Link to={"/transaction/" +  object.transactionId }><span className="truncate">{object.transactionId }</span></Link> </td>
                      {/* <td><pre>{JSON.stringify(object, null, 2) }</pre></td> */}
                      <td>{object.outputs.length > 1 ? object.outputs.reduce(function(a,b){ return {totalBalance: a.balance + b.balance}}).totalBalance : (object.outputs[0] ? object.outputs[0].balance : '')}</td>
                      <td>
                        {object.inputs
                          .map(function(input, j){
                            return <div>[{input.inputIndex}] {input.inputAddress === ''?'Unknown address':input.inputAddress} <i className="pull-right">{input.balance}</i></div>
                          })}
                      </td>
                      <td>
                        {object.outputs
                          .map(function(output, k){
                            return <div>[{output.index}] <Link to={"/address/" +  (output.address) }>{output.address}</Link> <i className="pull-right">{output.balance}</i></div>
                          })}
                      </td>
                  </tr>
                } )}
          </tbody>
          </table>
        </div>
      </Grid>
    );
  }
}
export default Block;