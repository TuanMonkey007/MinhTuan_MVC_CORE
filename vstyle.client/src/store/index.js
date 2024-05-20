// src/store/index.js
import { createStore } from 'vuex';

export default createStore({
  state: {
    loading: false
  },
  mutations: {
    SET_LOADING(state, payload) {
      state.loading = payload;
    }
  },
  actions: {
    setLoading({ commit }, payload) {
      commit('SET_LOADING', payload);
    }
  },
  getters: {
    isLoading: state => state.loading
  }
});
