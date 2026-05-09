if (!localStorage.getItem('biblioteca_token')) {
  window.location.href = 'login.html';
}

document.addEventListener('DOMContentLoaded', () => {
  // Navigation handling
  const navLinks = document.querySelectorAll('.sidebar-menu a');
  const sections = document.querySelectorAll('.content-section');

  function showSection(sectionId) {
    sections.forEach(section => {
      section.style.display = section.id === sectionId ? 'block' : 'none';
    });

    navLinks.forEach(link => {
      if (link.dataset.target === sectionId) {
        link.classList.add('active');
      } else {
        link.classList.remove('active');
      }
    });
  }

  // Permite chamada global para os botões do dashboard
  window.goToSection = function(sectionId) {
    const link = document.querySelector(`[data-target='${sectionId}']`);
    if (link) link.click();
  };

  navLinks.forEach(link => {
    link.addEventListener('click', (e) => {
      e.preventDefault();
      const target = link.dataset.target;
      showSection(target);
      
      // Carregar dados dependendo da aba
      if (target === 'dashboard') loadDashboard();
      if (target === 'livros') loadLivros();
      if (target === 'autores') loadAutores();
      if (target === 'categorias') loadCategorias();
      if (target === 'editoras') loadEditoras();
      if (target === 'usuarios') loadUsuarios();
    });
  });

  // Modal helpers
  window.openModal = function(modalId) {
    document.getElementById(modalId).classList.add('active');
  }

  window.closeModal = function(modalId) {
    document.getElementById(modalId).classList.remove('active');
  }

  document.querySelectorAll('.modal-close').forEach(btn => {
    btn.addEventListener('click', (e) => {
      const modal = e.target.closest('.modal-overlay');
      modal.classList.remove('active');
    });
  });

  // Load initial data
  showSection('dashboard');
  loadDashboard();
});

// --- DASHBOARD ---
async function loadDashboard() {
  try {
    const [livros, autores, categorias, editoras, usuarios] = await Promise.all([
      window.api.livros.getAll(),
      window.api.autores.getAll(),
      window.api.categorias.getAll(),
      window.api.editoras.getAll(),
      window.api.usuarios.getAll()
    ]);

    document.getElementById('total-livros').textContent = livros ? livros.length : 0;
    document.getElementById('total-autores').textContent = autores ? autores.length : 0;
    document.getElementById('total-categorias').textContent = categorias ? categorias.length : 0;
    document.getElementById('total-editoras').textContent = editoras ? editoras.length : 0;
    document.getElementById('total-usuarios').textContent = usuarios ? usuarios.length : 0;
  } catch (err) {
    console.error("Erro ao carregar dashboard", err);
  }
}

// --- LIVROS ---
async function loadLivros() {
  const tbody = document.getElementById('livros-tbody');
  tbody.innerHTML = '<tr><td colspan="6">Carregando...</td></tr>';
  
  try {
    const livros = await window.api.livros.getAll();
    tbody.innerHTML = '';
    
    if (!livros || livros.length === 0) {
      tbody.innerHTML = '<tr><td colspan="6">Nenhum livro encontrado.</td></tr>';
      return;
    }
    
    livros.forEach(livro => {
      const categorias = livro.categorias ? livro.categorias.join(', ') : '-';
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${livro.nomeLivro || '-'}</td>
        <td>${livro.nomeAutor || '-'}</td>
        <td>${livro.nomeEditora || '-'}</td>
        <td>${livro.anoPublicacao || '-'}</td>
        <td>${categorias}</td>
        <td>
          <button class="btn" style="padding: 4px 8px; font-size: 0.8rem;" onclick="openLivroModal('${livro.idLivro}')">Editar</button>
          <button class="btn btn-danger" style="padding: 4px 8px; font-size: 0.8rem;" onclick="deleteLivro('${livro.idLivro}')">Excluir</button>
        </td>
      `;
      tbody.appendChild(tr);
    });
  } catch (err) {
    tbody.innerHTML = '<tr><td colspan="6">Erro ao carregar livros.</td></tr>';
  }
}

// --- AUTORES ---
async function loadAutores() {
  const tbody = document.getElementById('autores-tbody');
  tbody.innerHTML = '<tr><td colspan="4">Carregando...</td></tr>';
  try {
    const autores = await window.api.autores.getAll();
    tbody.innerHTML = '';
    if (!autores || autores.length === 0) {
      tbody.innerHTML = '<tr><td colspan="4">Nenhum autor encontrado.</td></tr>';
      return;
    }
    autores.forEach(autor => {
      const dataNasc = autor.dataNascimento ? autor.dataNascimento.split('T')[0] : '-';
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${autor.primeiroNome || ''} ${autor.sobrenome || ''}</td>
        <td>${autor.nacionalidade || '-'}</td>
        <td>${dataNasc}</td>
        <td>
          <button class="btn" style="padding: 4px 8px; font-size: 0.8rem;" onclick="openAutorModal('${autor.idAutor}')">Editar</button>
          <button class="btn btn-danger" style="padding: 4px 8px; font-size: 0.8rem;" onclick="deleteAutor('${autor.idAutor}')">Excluir</button>
        </td>
      `;
      tbody.appendChild(tr);
    });
  } catch (err) {
    tbody.innerHTML = '<tr><td colspan="4">Erro ao carregar autores.</td></tr>';
  }
}

// --- CATEGORIAS ---
async function loadCategorias() {
  const tbody = document.getElementById('categorias-tbody');
  if(!tbody) return;
  tbody.innerHTML = '<tr><td colspan="3">Carregando...</td></tr>';
  try {
    const categorias = await window.api.categorias.getAll();
    tbody.innerHTML = '';
    categorias.forEach(cat => {
      tbody.innerHTML += `<tr>
        <td>${cat.nomeCategoria || '-'}</td>
        <td>${cat.descricaoCategoria || '-'}</td>
        <td>
          <button class="btn" style="padding: 4px 8px; font-size: 0.8rem;" onclick="openCategoriaModal('${cat.idCategoria}')">Editar</button>
          <button class="btn btn-danger" style="padding: 4px 8px; font-size: 0.8rem;" onclick="deleteCategoria('${cat.idCategoria}')">Excluir</button>
        </td>
      </tr>`;
    });
  } catch(e) {}
}

// --- EDITORAS ---
async function loadEditoras() {
  const tbody = document.getElementById('editoras-tbody');
  if(!tbody) return;
  tbody.innerHTML = '<tr><td colspan="5">Carregando...</td></tr>';
  try {
    const editoras = await window.api.editoras.getAll();
    tbody.innerHTML = '';
    editoras.forEach(ed => {
      const siteLink = ed.siteOficial ? `<a href="${ed.siteOficial}" target="_blank">Site</a>` : '-';
      tbody.innerHTML += `<tr>
        <td>${ed.nomeEditora || '-'}</td>
        <td>${ed.paisOrigem || '-'}</td>
        <td>${ed.anoFundacao || '-'}</td>
        <td>${siteLink}</td>
        <td>
          <button class="btn" style="padding: 4px 8px; font-size: 0.8rem;" onclick="openEditoraModal('${ed.idEditora}')">Editar</button>
          <button class="btn btn-danger" style="padding: 4px 8px; font-size: 0.8rem;" onclick="deleteEditora('${ed.idEditora}')">Excluir</button>
        </td>
      </tr>`;
    });
  } catch(e) {}
}

// --- USUÁRIOS ---
async function loadUsuarios() {
  const tbody = document.getElementById('usuarios-tbody');
  if(!tbody) return;
  tbody.innerHTML = '<tr><td colspan="4">Carregando...</td></tr>';
  try {
    const usuarios = await window.api.usuarios.getAll();
    tbody.innerHTML = '';
    if (!usuarios || usuarios.length === 0) {
      tbody.innerHTML = '<tr><td colspan="4">Nenhum usuário encontrado.</td></tr>';
      return;
    }
    usuarios.forEach(user => {
      const status = user.emailConfirmed ? '<span style="color:var(--primary-color)">Ativo</span>' : '<span style="color:var(--text-secondary)">Pendente</span>';
      tbody.innerHTML += `<tr>
        <td>${user.username || '-'}</td>
        <td>${user.email || '-'}</td>
        <td>${status}</td>
        <td>
          <button class="btn" style="padding: 4px 8px; font-size: 0.8rem;" onclick="openUsuarioModal('${user.id}')">Editar</button>
          <button class="btn btn-danger" style="padding: 4px 8px; font-size: 0.8rem;" onclick="deleteUsuario('${user.id}')">Excluir</button>
        </td>
      </tr>`;
    });
  } catch(err) {
    tbody.innerHTML = '<tr><td colspan="4">Erro ao carregar usuários.</td></tr>';
  }
}

// --- ACTIONS / DELETE ---
window.deleteLivro = async function(id) {
  if(confirm('Deseja realmente excluir este livro?')) {
    try {
      await window.api.livros.delete(id);
      loadLivros();
      loadDashboard();
    } catch(err) { }
  }
}

window.deleteAutor = async function(id) {
  if(confirm('Deseja realmente excluir este autor?')) {
    try {
      await window.api.autores.delete(id);
      loadAutores();
      loadDashboard();
    } catch(err) { }
  }
}

window.deleteCategoria = async function(id) {
  if(confirm('Deseja realmente excluir esta categoria?')) {
    try {
      await window.api.categorias.delete(id);
      loadCategorias();
      loadDashboard();
    } catch(err) { }
  }
}

window.deleteEditora = async function(id) {
  if(confirm('Deseja realmente excluir esta editora?')) {
    try {
      await window.api.editoras.delete(id);
      loadEditoras();
      loadDashboard();
    } catch(err) { }
  }
}

window.deleteUsuario = async function(id) {
  if(confirm('Deseja realmente excluir este usuário?')) {
    try {
      await window.api.usuarios.delete(id);
      loadUsuarios();
      loadDashboard();
    } catch(err) { }
  }
}

// --- ACTIONS / MODALS (EDIT/CREATE) ---
window.openLivroModal = async function(id = null) {
  const form = document.getElementById('form-livro');
  form.reset();
  document.getElementById('livro-id').value = id || '';
  document.getElementById('modal-livro-title').textContent = id ? 'Editar Livro' : 'Novo Livro';
  
  if (id) {
    try {
      const livro = await window.api.livros.getById(id);
      document.getElementById('livro-titulo').value = livro.nomeLivro || '';
      document.getElementById('livro-autor').value = livro.idAutor || '';
      document.getElementById('livro-editora').value = livro.idEditora || '';
      document.getElementById('livro-ano').value = livro.anoPublicacao || '';
    } catch(err) { }
  }
  openModal('modal-livro');
}

window.openAutorModal = async function(id = null) {
  const form = document.getElementById('form-autor');
  form.reset();
  document.getElementById('autor-id').value = id || '';
  document.getElementById('modal-autor-title').textContent = id ? 'Editar Autor' : 'Novo Autor';
  
  if (id) {
    try {
      const autor = await window.api.autores.getById(id);
      document.getElementById('autor-primeironome').value = autor.primeiroNome || '';
      document.getElementById('autor-sobrenome').value = autor.sobrenome || '';
      document.getElementById('autor-nacionalidade').value = autor.nacionalidade || '';
      if(autor.dataNascimento) {
        document.getElementById('autor-nascimento').value = autor.dataNascimento.split('T')[0];
      }
    } catch(err) { }
  }
  openModal('modal-autor');
}

window.openCategoriaModal = async function(id = null) {
  const form = document.getElementById('form-categoria');
  form.reset();
  document.getElementById('categoria-id').value = id || '';
  document.getElementById('modal-categoria-title').textContent = id ? 'Editar Categoria' : 'Nova Categoria';
  
  if (id) {
    try {
      const cat = await window.api.categorias.getById(id);
      document.getElementById('categoria-nome').value = cat.nomeCategoria || '';
      document.getElementById('categoria-descricao').value = cat.descricaoCategoria || '';
    } catch(err) { }
  }
  openModal('modal-categoria');
}

window.openEditoraModal = async function(id = null) {
  const form = document.getElementById('form-editora');
  form.reset();
  document.getElementById('editora-id').value = id || '';
  document.getElementById('modal-editora-title').textContent = id ? 'Editar Editora' : 'Nova Editora';
  
  if (id) {
    try {
      const ed = await window.api.editoras.getById(id);
      document.getElementById('editora-nome').value = ed.nomeEditora || '';
      document.getElementById('editora-pais').value = ed.paisOrigem || '';
      document.getElementById('editora-ano').value = ed.anoFundacao || '';
      document.getElementById('editora-site').value = ed.siteOficial || '';
    } catch(err) { }
  }
  openModal('modal-editora');
}

window.openUsuarioModal = async function(id = null) {
  const form = document.getElementById('form-usuario');
  form.reset();
  document.getElementById('usuario-id').value = id || '';
  document.getElementById('modal-usuario-title').textContent = id ? 'Editar Usuário' : 'Novo Usuário';
  
  // Senha só é obrigatória no cadastro
  document.getElementById('usuario-password').required = !id;
  
  if (id) {
    try {
      const user = await window.api.usuarios.getById(id);
      document.getElementById('usuario-username').value = user.username || '';
      document.getElementById('usuario-email').value = user.email || '';
    } catch(err) { }
  }
  openModal('modal-usuario');
}

// --- FORM SUBMITS ---
document.getElementById('form-livro')?.addEventListener('submit', async (e) => {
  e.preventDefault();
  const id = document.getElementById('livro-id').value;
  const data = {
    idLivro: id ? parseInt(id) : 0,
    nomeLivro: document.getElementById('livro-titulo').value,
    idAutor: parseInt(document.getElementById('livro-autor').value),
    idEditora: parseInt(document.getElementById('livro-editora').value),
    anoPublicacao: parseInt(document.getElementById('livro-ano').value)
  };
  
  try {
    if (id) await window.api.livros.update(id, data);
    else await window.api.livros.create(data);
    closeModal('modal-livro');
    loadLivros();
    loadDashboard();
  } catch(err) { }
});

document.getElementById('form-autor')?.addEventListener('submit', async (e) => {
  e.preventDefault();
  const id = document.getElementById('autor-id').value;
  const data = {
    idAutor: id ? parseInt(id) : 0,
    primeiroNome: document.getElementById('autor-primeironome').value,
    sobrenome: document.getElementById('autor-sobrenome').value,
    nacionalidade: document.getElementById('autor-nacionalidade').value,
    dataNascimento: document.getElementById('autor-nascimento').value
  };
  
  try {
    if (id) await window.api.autores.update(id, data);
    else await window.api.autores.create(data);
    closeModal('modal-autor');
    loadAutores();
    loadDashboard();
  } catch(err) { }
});

document.getElementById('form-categoria')?.addEventListener('submit', async (e) => {
  e.preventDefault();
  const id = document.getElementById('categoria-id').value;
  const data = {
    idCategoria: id ? parseInt(id) : 0,
    nomeCategoria: document.getElementById('categoria-nome').value,
    descricaoCategoria: document.getElementById('categoria-descricao').value
  };
  
  try {
    if (id) await window.api.categorias.update(id, data);
    else await window.api.categorias.create(data);
    closeModal('modal-categoria');
    loadCategorias();
    loadDashboard();
  } catch(err) { }
});

document.getElementById('form-editora')?.addEventListener('submit', async (e) => {
  e.preventDefault();
  const id = document.getElementById('editora-id').value;
  const data = {
    idEditora: id ? parseInt(id) : 0,
    nomeEditora: document.getElementById('editora-nome').value,
    paisOrigem: document.getElementById('editora-pais').value,
    anoFundacao: parseInt(document.getElementById('editora-ano').value),
    siteOficial: document.getElementById('editora-site').value
  };
  
  try {
    if (id) await window.api.editoras.update(id, data);
    else await window.api.editoras.create(data);
    closeModal('modal-editora');
    loadEditoras();
    loadDashboard();
  } catch(err) { }
});

document.getElementById('form-usuario')?.addEventListener('submit', async (e) => {
  e.preventDefault();
  const id = document.getElementById('usuario-id').value;
  
  if (id) {
    // Update
    const data = {
      id: id,
      username: document.getElementById('usuario-username').value,
      email: document.getElementById('usuario-email').value
    };
    try {
      await window.api.usuarios.update(id, data);
      closeModal('modal-usuario');
      loadUsuarios();
      loadDashboard();
    } catch(err) { }
  } else {
    // Create (Register)
    const data = {
      nomeUsuario: document.getElementById('usuario-username').value,
      emailUsuario: document.getElementById('usuario-email').value,
      password: document.getElementById('usuario-password').value
    };
    try {
      await window.api.usuarios.create(data);
      closeModal('modal-usuario');
      loadUsuarios();
      loadDashboard();
    } catch(err) { }
  }
});
