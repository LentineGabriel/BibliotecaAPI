const API_URL = 'https://localhost:7012/v1';

/**
 * Função utilitária para fazer requisições à API
 */
async function fetchAPI(endpoint, method = 'GET', body = null) {
  const options = {
    method,
    headers: {
      'Content-Type': 'application/json'
    }
  };

  // Se o token existir, adiciona o Authorization Header
  const token = localStorage.getItem('biblioteca_token');
  if (token) {
    options.headers['Authorization'] = `Bearer ${token}`;
  }

  if (body) {
    options.body = JSON.stringify(body);
  }

  try {
    const response = await fetch(`${API_URL}${endpoint}`, options);
    
    // Redireciona para o login caso o token seja inválido (401) ou não tenha permissão (403)
    if (response.status === 401 || response.status === 403) {
      localStorage.removeItem('biblioteca_token');
      window.location.href = 'login.html';
      return null;
    }

    if (!response.ok) {
      const errorData = await response.text();
      throw new Error(errorData || `Erro HTTP: ${response.status}`);
    }
    
    // Tratamento para 204 No Content (ex: Deletes ou Updates que não retornam corpo)
    if (response.status === 204) return null;
    
    return await response.json();
  } catch (error) {
    console.error('API Error:', error);
    if (!window.location.href.includes('login.html')) {
        alert('Erro ao comunicar com a API: ' + error.message);
    }
    throw error;
  }
}

// Objeto que concentra as chamadas para facilitar a manutenção
window.api = {
  auth: {
    login: (username, password) => fetchAPI('/Users/LoginUsuario', 'POST', { nomeUsuario: username, password })
  },
  livros: {
    getAll: () => fetchAPI('/Livros'),
    getById: (id) => fetchAPI(`/Livros/${id}`),
    create: (data) => fetchAPI('/Livros/AdicionarLivro', 'POST', data),
    update: (id, data) => fetchAPI(`/Livros/AtualizarLivro/${id}`, 'PUT', data),
    delete: (id) => fetchAPI(`/Livros/DeletarLivros/${id}`, 'DELETE')
  },
  autores: {
    getAll: () => fetchAPI('/Autor'),
    getById: (id) => fetchAPI(`/Autor/${id}`),
    create: (data) => fetchAPI('/Autor/AdicionarAutores', 'POST', data),
    update: (id, data) => fetchAPI(`/Autor/AtualizarAutor/${id}`, 'PUT', data),
    delete: (id) => fetchAPI(`/Autor/DeletarAutor/${id}`, 'DELETE')
  },
  categorias: {
    getAll: () => fetchAPI('/Categorias'),
    getById: (id) => fetchAPI(`/Categorias/${id}`),
    create: (data) => fetchAPI('/Categorias/AdicionarCategorias', 'POST', data),
    update: (id, data) => fetchAPI(`/Categorias/AtualizarCategoria/${id}`, 'PUT', data),
    delete: (id) => fetchAPI(`/Categorias/DeletarCategoria/${id}`, 'DELETE')
  },
  editoras: {
    getAll: () => fetchAPI('/Editoras'),
    getById: (id) => fetchAPI(`/Editoras/${id}`),
    create: (data) => fetchAPI('/Editoras/AdicionarEditoras', 'POST', data),
    update: (id, data) => fetchAPI(`/Editoras/AtualizarEditora/${id}`, 'PUT', data),
    delete: (id) => fetchAPI(`/Editoras/DeletarEditora/${id}`, 'DELETE')
  },
  usuarios: {
    getAll: () => fetchAPI('/Users'),
    getById: (id) => fetchAPI(`/Users/${id}`),
    create: (data) => fetchAPI('/Users/RegistrarUsuario', 'POST', data),
    update: (id, data) => fetchAPI(`/Users/AtualizarUsuario/${id}`, 'PUT', data),
    delete: (id) => fetchAPI(`/Users/DeletarUsuario/${id}`, 'DELETE')
  }
};
