import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
});

export default api;


/// cum se foloseste in componentele sale
// import api from './services/api';

// api.get('/topics').then(response => {
//   console.log(response.data);
// });
