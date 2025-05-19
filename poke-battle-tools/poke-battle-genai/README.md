# 🧠 Proyecto GenAI - NPC Pokémon (v1)

Este proyecto es una integración de **IA generativa local con Ollama**, usando modelos como `llama3`, conectada a un backend **ASP.NET Core Web API** y un frontend en **Vue 3**, con el objetivo de simular un NPC inteligente estilo "Profesor Oak" que responde preguntas sobre Pokémon en **español** y con formato **HTML renderizado dinámicamente**.

---

## 🚀 Tecnologías usadas

- 🧠 [Ollama](http://ollama.com) (`llama3`): modelo de lenguaje local corriendo en tu máquina
- 🧱 ASP.NET Core: backend para recibir prompts y gestionar memoria por sesión
- 🌐 Vue 3: frontend que muestra respuestas generadas en HTML
- 🧠 Semantic Kernel: para estructurar agentes conversacionales
- 🧪 ChatHistory persistente por usuario (en memoria)

---

## ✅ Funcionalidades implementadas (v1)

- [x] Chatbot estilo NPC (Profesor Oak) en español
- [x] Respuestas en formato **HTML** para renderizado visual
- [x] Conexión con **Ollama local** desde backend ASP.NET
- [x] Almacenamiento temporal de conversaciones por usuario (`ChatHistory`)
- [x] Renderizado en Vue usando `v-html`

---

## 🖼️ Captura de pantalla
![image](https://github.com/user-attachments/assets/dff1e775-fc0b-4e58-a66c-b5ba390afc2e)

---
## 📦 Estructura general

```plaintext
/ollama-server        → Ollama corriendo modelo LLaMA3
/backend              → ASP.NET Core API con Semantic Kernel
/frontend             → Vue 3 app que muestra respuestas IA
