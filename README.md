# 🍎 Hortifruti RabbitMQ Microservices

Projeto desenvolvido para o **Checkpoint 5** da disciplina de **Programação de API com Microservices e Mensageria** da FIAP.

---

# 👨‍🏫 Professor

**Dr. Marcel Stefan Wagner**

---

# 👩‍💻 Integrantes

* Maria Eduarda Araujo Penas
* Alane Rocha

---

# 📚 Descrição do Projeto

Este projeto tem como objetivo demonstrar a implementação de um sistema distribuído utilizando:

* RabbitMQ
* Mensageria
* Microservices
* Producers e Consumers
* Validação de mensagens
* Docker
* C#
* .NET 8

O sistema simula um ambiente de gestão de um Hortifruti, onde mensagens são enviadas, validadas e posteriormente consumidas por serviços específicos.

---

# 🏗️ Arquitetura do Projeto

O projeto foi dividido em múltiplos microservices:

| Projeto           | Responsabilidade                  |
| ----------------- | --------------------------------- |
| SenderFrutas      | Enviar informações sobre frutas   |
| SenderUsuarios    | Enviar informações sobre usuários |
| ValidationService | Validar mensagens recebidas       |
| ReceiverFrutas    | Receber frutas validadas          |
| ReceiverUsuarios  | Receber usuários validados        |
| Shared            | Classes compartilhadas            |

---

# 🔄 Fluxo da Aplicação

## 🍓 Fluxo de Frutas

SenderFrutas
↓
RabbitMQ Exchange Input
↓
ValidationService
↓
RabbitMQ Exchange Validated
↓
ReceiverFrutas

---

## 👤 Fluxo de Usuários

SenderUsuarios
↓
RabbitMQ Exchange Input
↓
ValidationService
↓
RabbitMQ Exchange Validated
↓
ReceiverUsuarios

---

# 🐇 RabbitMQ

O RabbitMQ foi utilizado como **Message Broker** para realizar a comunicação assíncrona entre os microservices.

O sistema foi responsável por:

* Gerenciamento das filas
* Distribuição das mensagens
* Comunicação entre Producers e Consumers
* Controle das exchanges
* Encaminhamento através das routing keys

---

# 🐳 Docker

O RabbitMQ foi executado utilizando **Docker Desktop**, permitindo maior facilidade na configuração do ambiente.

## 📌 Portas Utilizadas

| Porta | Descrição           |
| ----- | ------------------- |
| 5672  | Comunicação AMQP    |
| 15672 | Painel Web RabbitMQ |

---

# ⚙️ Tecnologias Utilizadas

* C#
* .NET 8
* RabbitMQ.Client
* RabbitMQ
* Docker Desktop
* Visual Studio Code
* GitHub

---

# 📁 Estrutura do Projeto

hortifruti-rabbitmq-microservices/

├── SenderFrutas
├── SenderUsuarios
├── ValidationService
├── ReceiverFrutas
├── ReceiverUsuarios
├── Shared
│
├── docker-compose.yml
├── README.md
└── hortifruti-rabbitmq-microservices.sln

---

# 📦 Exchanges

## 📥 Exchange de Entrada

hortifruti.exchange.input

Responsável pelo recebimento inicial das mensagens enviadas pelos producers.

---

## ✅ Exchange de Validação

hortifruti.exchange.validated

Responsável pelo envio das mensagens após validação.

---

# 📬 Queues

| Queue                     | Função                          |
| ------------------------- | ------------------------------- |
| queue.frutas.validation   | Receber frutas para validação   |
| queue.usuarios.validation | Receber usuários para validação |
| queue.frutas.receiver     | Receber frutas validadas        |
| queue.usuarios.receiver   | Receber usuários validados      |

---

# 🔑 Routing Keys

| Routing Key        | Função              |
| ------------------ | ------------------- |
| frutas.input       | Entrada de frutas   |
| usuarios.input     | Entrada de usuários |
| frutas.validated   | Frutas validadas    |
| usuarios.validated | Usuários validados  |

---

# ✅ Processo de Validação

## 🍎 Validação de Frutas

O ValidationService valida:

* Nome da fruta
* Descrição da fruta

Caso os dados estejam válidos:

* a mensagem é reenviada para a exchange de mensagens validadas.

---

## 👤 Validação de Usuários

O ValidationService valida:

* Nome completo
* CPF

Caso os dados estejam válidos:

* a mensagem é reenviada para a exchange de mensagens validadas.

---

# 🚀 Como Executar o Projeto

## 1️⃣ Clonar o Repositório

git clone LINK_DO_REPOSITORIO

---

## 2️⃣ Entrar na Pasta do Projeto

cd hortifruti-rabbitmq-microservices

---

## 3️⃣ Subir RabbitMQ com Docker

docker-compose up -d

---

## 4️⃣ Acessar RabbitMQ

Abrir navegador:

http://localhost:15672

### 🔐 Login

Usuário: guest
Senha: guest

---

# ▶️ Execução dos Microservices

## 5️⃣ Executar ValidationService

cd ValidationService
dotnet run

---

## 6️⃣ Executar ReceiverFrutas

cd ReceiverFrutas
dotnet run

---

## 7️⃣ Executar ReceiverUsuarios

cd ReceiverUsuarios
dotnet run

---

## 8️⃣ Executar SenderFrutas

cd SenderFrutas
dotnet run

---

## 9️⃣ Executar SenderUsuarios

cd SenderUsuarios
dotnet run

---

# 🧪 Exemplos de Execução

## 🍓 SenderFrutas

INICIANDO SENDER DE FRUTAS...

MENSAGEM ENVIADA COM SUCESSO!

---

## ✅ ValidationService

FRUTA RECEBIDA:

VALIDAÇÃO OK!

MENSAGEM VALIDADA ENVIADA!

---

## 📥 ReceiverFrutas

MENSAGEM VALIDADA RECEBIDA:

---

## 👤 SenderUsuarios

INICIANDO SENDER DE USUÁRIOS...

USUÁRIO ENVIADO COM SUCESSO!

---

## 📥 ReceiverUsuarios

USUÁRIO VALIDADO RECEBIDO:

---

# 🔍 Monitoramento RabbitMQ

Durante a execução do sistema foi possível visualizar:

* Connections
* Channels
* Exchanges
* Queues
* Consumers

diretamente no painel administrativo do RabbitMQ.

---

# 📸 Evidências

Adicionar neste espaço:

* Prints das Exchanges
* Prints das Queues
* Prints dos Channels
* Prints dos Consumers
* Prints dos terminais executando

---

# 🎯 Objetivo Acadêmico

O projeto teve como objetivo aplicar conceitos de:

* Comunicação assíncrona
* Mensageria
* Arquitetura distribuída
* Microservices
* RabbitMQ
* Docker
* Integração entre serviços

---

# ✅ Conclusão

O projeto permitiu compreender o funcionamento da comunicação entre microservices utilizando RabbitMQ, além da implementação prática de:

* Exchanges
* Queues
* Routing Keys
* Producers
* Consumers
* Docker
* Validação de mensagens
* Comunicação assíncrona

O desenvolvimento possibilitou consolidar conhecimentos sobre arquitetura distribuída e mensageria em aplicações modernas.

---

# 📌 Disciplina

Programação de API com Microservices e Mensageria

FIAP - Faculdade de Informática e Administração Paulista
