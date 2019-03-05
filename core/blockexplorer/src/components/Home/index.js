import React, { Component } from 'react';
import './style.css';
import '../../sbadmin2.css';
import Block from './../Block';
import { BrowserRouter as Router, Redirect, Route, Link } from 'react-router-dom'
import { Grid } from 'react-bootstrap'
//import { Grid, Row, Col, Table } from 'react-bootstrap'
import Moment from 'react-moment';

class Home extends Component {
    apiBaseUrl = '';

    constructor() {
        super();
        this.state={latestBlock:{}, blocks:[]};
        this.state.redirectUrl = null;
    }

    componentDidMount() {
        this.getLatestBlocks(15);
    }
    
    getLatestBlocks(numberOfBlocks) {
        fetch(`${this.apiBaseUrl}/api/query/block/latest`, { mode: 'cors' })
            .then(result => result.json())
            .then(latestBlock => this.setState({ latestBlock }))
            .then(async _ => {
                for (let i = 0; i < numberOfBlocks; i++) {
                    var currentTime = new Date().getTime();
                    while (currentTime + 10 >= new Date().getTime()) {
                        //stupid 10ms delay to help enforce order
                    }
                    let blockNum = this.state.latestBlock.blockIndex - i;

                    let url = `${this.apiBaseUrl}/api/query/block/Index/${blockNum}`;
                    await fetch(url, { mode: 'cors' })
                        .then(result => result.json())
                        .then(block => this.setState({ blocks: this.state.blocks.concat(block) }));
                }
            });
    }

    searchKeyPress = e => {
        console.log(e);
        if(e.keyCode == 13){
            var searchTerm = e.target.value;

            const isBlockNumber = parseInt(searchTerm) <= this.state.latestBlock.blockIndex;

            if (isBlockNumber) {
                this.setState({redirectUrl:'/block/' + searchTerm});
            }
        }
    }

    render() {
        if (this.state.redirectUrl) {
            return <Redirect to={this.state.redirectUrl}/>
        }
        
        return (
            <Grid>
                <div className="Home">
                    
                
                    <div className="row">
                        <div class="col-md-1 logo"><img src='/nako_logo.png' width="60" /></div>
                        <div className="col-md-11"><input class="pull-right search form-control form-control-lg" onKeyDown={this.searchKeyPress} type="text" placeholder="Search for block number"></input></div>
                    </div>
                    <div className="well">
                        <h1>{this.state.latestBlock.coinTag} Block explorer</h1>
                    </div>
               
                    
                <div className="row">
                <div className="col-lg-3 col-md-6">
                    <div className="panel panel-primary">
                        <div className="panel-heading">
                            <div className="row">
                                <div className="col-xs-3">
                                    <i className="fa fa-comments fa-5x"></i>
                                </div>
                                <div className="col-xs-9 text-right">
                                    <div className="huge">{this.state.latestBlock.blockIndex}</div>
                                    <div>Block Height</div>
                                </div>
                            </div>
                        </div>
                       
                            <div className="panel-footer">
                                <span className="pull-left"><Link to={"/block/" +  this.state.latestBlock.blockIndex }> View latest </Link></span>
                                <span className="pull-right"><i className="fa fa-arrow-circle-right"></i></span>
                                <div className="clearfix"></div>
                            </div>
                       
                    </div>
                </div>
                {/* <div className="col-lg-3 col-md-6">
                    <div className="panel panel-green">
                        <div className="panel-heading">
                            <div className="row">
                                <div className="col-xs-3">
                                    <i className="fa fa-tasks fa-5x"></i>
                                </div>
                                <div className="col-xs-9 text-right">
                                    <div className="huge">$???.??m</div>
                                    <div>Market Cap</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div className="panel-footer">
                                <span className="pull-left">View Details</span>
                                <span className="pull-right"><i className="fa fa-arrow-circle-right"></i></span>
                                <div className="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div className="col-lg-3 col-md-6">
                    <div className="panel panel-yellow">
                        <div className="panel-heading">
                            <div className="row">
                                <div className="col-xs-3">
                                    <i className="fa fa-shopping-cart fa-5x"></i>
                                </div>
                                <div className="col-xs-9 text-right">
                                    <div className="huge">?</div>
                                    <div>Known Peers</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div className="panel-footer">
                                <span className="pull-left">View Details</span>
                                <span className="pull-right"><i className="fa fa-arrow-circle-right"></i></span>
                                <div className="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div className="col-lg-3 col-md-6">
                    <div className="panel panel-red">
                        <div className="panel-heading">
                            <div className="row">
                                <div className="col-xs-3">
                                    <i className="fa fa-support fa-5x"></i>
                                </div>
                                <div className="col-xs-9 text-right">
                                    <div className="huge"><span style={{fontSize:30 + 'px'}}>{parseInt(this.state.latestBlock.blockIndex) + 6000000000 }</span></div>
                                    <div>Total supply of {this.state.latestBlock.coinTag}</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div className="panel-footer">
                                <span className="pull-left">View Details</span>
                                <span className="pull-right"><i className="fa fa-arrow-circle-right"></i></span>
                                <div className="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div> */}
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
                                <td>Tx Count</td>
                                <td>Size</td>
                            </tr>
                        </thead>
                        <tbody>
                            {this.state.blocks
                            .map(function(object, i){
                                return <tr key={i}>
                                    <td><Link to={"/block/" +  object.blockIndex }> {object.blockIndex}</Link></td>
                                    <td><Moment fromNow ago unix>{object.blockTime}</Moment></td>
                                    <td>{object.blockHash}</td>
                                    <td>{object.transactionCount}</td>
                                    <td>{object.blockSize} bytes</td>
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
