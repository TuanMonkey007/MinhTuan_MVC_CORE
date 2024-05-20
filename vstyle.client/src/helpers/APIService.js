import axios from 'axios';

/* eslint-disable */

axios.defaults.baseURL = process.env.VUE_APP_URL;
const APIService = {
  get: async function(endpoint) {
    try {
      let bearerToken = localStorage.getItem('accessToken');
      if(bearerToken){
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + bearerToken;
      }
      const response = await axios.get(`${endpoint}`);
      return response;
    } catch (e) {
      throw e;
    }
  },

  post: async function(endpoint, data = {}, ops = {}) {
    try {
      let bearerToken = localStorage.getItem('accessToken');
      if(bearerToken){
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + bearerToken;
      }
      const response = await axios.post(`${endpoint}`, data, ops);
      return response;
    } catch (e) {
      throw e;
    }
  },

  put: async function(endpoint, data = {}) {
    try {
      let bearerToken = localStorage.getItem('accessToken');
      if(bearerToken){
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + bearerToken;
      }
      const response = await axios.put(`${endpoint}`, data);
      return response;
    } catch (error) {
      throw error
    }
  },

  delete: async function(endpoint) {
    try {
      let bearerToken = localStorage.getItem('accessToken');
      if(bearerToken){
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + bearerToken;
      }
      const response = await axios.delete(`${endpoint}`);
      return response;
    } catch (error) {
      throw error
    }
  }
};

export default APIService;
//file này để làm gì ?