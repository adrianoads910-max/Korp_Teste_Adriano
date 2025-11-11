
# 🚀 Korp Faturamento

Sistema de faturamento desenvolvido para gerenciamento financeiro, com autenticação de usuários (login e logout), cadastro e controle de faturas.

## 📌 Tecnologias utilizadas

### Backend
- .NET 8 (Web API)
- Entity Framework Core
- SQLite

### Frontend
- React / Typescript (ou colocar aqui a sua tecnologia real)
- Axios
- Styled Components / Tailwind (se aplicável)

---

## 🗂 Estrutura do projeto

```

├── backend/
│   ├── FaturamentoService/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Data/
│   │   ├── Migrations/
│   │   └── ...
└── frontend/

````

---

## ⚙️ Como executar o projeto

### ✅ Backend (.NET)

```sh
cd backend/FaturamentoService
dotnet restore
dotnet ef database update
dotnet run
````

### ✅ Frontend (React)

```sh
cd frontend
npm install
npm start
```

Após iniciar, abra no navegador:

```
http://localhost:3000
```

---

## 🔐 Autenticação

O projeto possui:

* Registro de usuário
* Login
* Logout
* Geração e validação de token JWT

---

## 🧪 Endpoints (API)

| Método | Rota             | Descrição                          |
| ------ | ---------------- | ---------------------------------- |
| POST   | `/auth/register` | Registra um novo usuário           |
| POST   | `/auth/login`    | Login e retorno do token           |
| GET    | `/faturas`       | Lista todas as faturas (com token) |

*(Coloque aqui os endpoints reais do seu projeto)*

---

## ✨ Melhorias futuras

* Implementar refresh token
* Upload de comprovantes
* Dashboard com gráficos

---

## 👨‍💻 Autor

**Adriano**
🔗 GitHub: [https://github.com/adrianoads910-max](https://github.com/adrianoads910-max)


---

## 🤝 Contribuição

Contribuições são bem-vindas! Se você quiser colaborar:

1. Faça um fork do repositório
2. Crie uma branch com a sua feature: `git checkout -b feature/nome-da-feature`
3. Faça suas alterações e commit: `git commit -m "feat: descrição da feature"`
4. Envie para o seu repositório: `git push origin feature/nome-da-feature`
5. Abra um Pull Request aqui no repositório original

---

## 📄 Licença

Este projeto está licenciado sob os termos da **MIT License** (ou outra licença de sua escolha).

