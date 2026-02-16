# Microserviço Serverless de Validação de CPF

Este repositório contém um microserviço para **validação de CPF** utilizando **Azure Functions** em uma arquitetura **serverless** na **Microsoft Azure**.

A função expõe um endpoint HTTP que recebe um CPF, normaliza o valor (remove pontos, traços e espaços) e valida os dígitos verificadores, retornando se o CPF é **válido** ou **inválido**.

---

## 🎯 Objetivo

Demonstrar, na prática, como:

- Criar um **microserviço serverless** usando **Azure Functions**  
- Implementar a **lógica de negócio** de validação de CPF  
- Disponibilizar um endpoint HTTP simples para consumo por outras aplicações  
- Manter um serviço **escalável**, **de baixo custo** e **fácil de manter** na nuvem Azure  

---

## 🧱 Arquitetura

- **Azure Functions** (HTTP Trigger, nível de autorização `Anonymous`)
- Padrão **serverless** (execução sob demanda, escalabilidade automática)
- Endpoint HTTP exposto via rota:

  ```text
  /api/validate-cpf
