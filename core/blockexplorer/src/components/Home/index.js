import React, { Component } from 'react';
import './style.css';
import '../../sbadmin2.css';
import Block from './../Block';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom'
import { Grid } from 'react-bootstrap'
//import { Grid, Row, Col, Table } from 'react-bootstrap'
import Moment from 'react-moment';

class Home extends Component {

    constructor() {
        super();
        this.state={latestBlock:{}, blocks:[]};
    }

    componentDidMount() {
        fetch(`/api/query/block/latest`,{mode: 'cors'})
            .then(result=>result.json())
            .then(latestBlock=>this.setState({latestBlock}))
            .then(_ => {
                for (let index = 0; index < 50; index++) {
                    var currentTime = new Date().getTime();
                    while (currentTime + 1 >= new Date().getTime()) {
                        //stupid 1ms delay to help enforce order
                    }
                    let blockNum = this.state.latestBlock.blockIndex - index;
                    let url = `/api/query/block/Index/${blockNum}/transactions`;
                    fetch(url ,{mode: 'cors'})
                    .then(result=>result.json())
                    .then(block=>this.setState({blocks: this.state.blocks.concat(block)}));
                    
                }
            });
    }
    
    render() {
        return (
            <Grid>
                <div className="Home">
                    
                        <div className="jumbotron">
                            <h1>{this.state.latestBlock.coinTag} Block explorer</h1>
                        </div>
                    
                        <div class="row">
                <div class="col-lg-3 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-comments fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge">{this.state.latestBlock.blockIndex}</div>
                                    <div>Block Height</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left"><Link to={"/block/" +  this.state.latestBlock.blockIndex }> View latest </Link></span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="panel panel-green">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-tasks fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge">$???.??m</div>
                                    <div>Market Cap</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="panel panel-yellow">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-shopping-cart fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge">?</div>
                                    <div>Known Peers</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="panel panel-red">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-support fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge"><span style={{fontSize:30 + 'px'}}>{parseInt(this.state.latestBlock.blockIndex) + 6000000000 }</span></div>
                                    <div>Total supply of {this.state.latestBlock.coinTag}</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
            </div>
                        <div>
                            Current height: <Link to={"/block/" +  this.state.latestBlock.blockIndex }> {this.state.latestBlock.blockIndex}</Link>
                            <Route path="/block/:blockIndex"  component={Block}/>

                            
                        </div>
                        <table className="table table-bordered table-striped">
                        <thead>
                            <tr>
                                <td>Height</td>
                                <td>Age</td>
                                <td>Hash</td>
                            </tr>
                        </thead>
                        <tbody>
                            {this.state.blocks
                            .map(function(object, i){
                                return <tr key={i}>
                                    <td><Link to={"/block/" +  object.blockIndex }> {object.blockIndex}</Link></td>
                                    {/* <td>{object.transactionCount}</td> */}
                                    <td><Moment fromNow ago unix>{object.blockTime}</Moment></td>
                                    <td>{object.blockHash}</td>
                                </tr>
                             } )}
                        </tbody>
                        </table>
                    
                
            
                </div>
            </Grid>
        );
    }
}

export default Home;