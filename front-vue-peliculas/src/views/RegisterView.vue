<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { AxiosError } from 'axios'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()
const username = ref('')
const password = ref('')
const confirmPassword = ref('')
const errorMessage = ref('')
const isSubmitting = ref(false)
const isRegistered = ref(false)

async function submitRegister() {
  errorMessage.value = ''

  if (!username.value.trim() || !password.value || !confirmPassword.value) {
    errorMessage.value = 'Completa todos los campos.'
    return
  }

  if (password.value !== confirmPassword.value) {
    errorMessage.value = 'Las contraseñas no coinciden.'
    return
  }

  isSubmitting.value = true

  try {
    await authStore.register(username.value.trim(), password.value)
    isRegistered.value = true
  } catch (error) {
    const responseError = error as AxiosError<{ message?: string }>
    const message = responseError.response?.data?.message
    errorMessage.value = Array.isArray(message)
      ? message.join(' ')
      : message ?? 'No fue posible crear el usuario.'
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <main class="login-shell">
    <section class="login-panel" aria-labelledby="register-title">
      <div class="brand-mark" aria-hidden="true">M</div>
      <p class="eyebrow">Movie Radar</p>
      <h1 id="register-title">Crea tu usuario</h1>
      <p class="login-intro">Regístrate para administrar tu catálogo de películas.</p>

      <div v-if="isRegistered" class="success-message" role="status">
        <strong>Usuario creado correctamente.</strong>
        <span>Ya puedes iniciar sesión con tus credenciales.</span>
      </div>

      <form v-else class="login-form" @submit.prevent="submitRegister" novalidate>
        <label for="register-username">Usuario</label>
        <input
          id="register-username"
          v-model="username"
          name="username"
          type="text"
          autocomplete="username"
          placeholder="Elige un usuario"
          required
        />

        <label for="register-password">Contraseña</label>
        <input
          id="register-password"
          v-model="password"
          name="password"
          type="password"
          autocomplete="new-password"
          placeholder="Crea una contraseña"
          required
        />

        <label for="confirm-password">Repite la contraseña</label>
        <input
          id="confirm-password"
          v-model="confirmPassword"
          name="confirm-password"
          type="password"
          autocomplete="new-password"
          placeholder="Repite tu contraseña"
          required
        />

        <p v-if="errorMessage" class="form-error" role="alert">
          {{ errorMessage }}
        </p>

        <button class="submit-button" type="submit" :disabled="isSubmitting">
          {{ isSubmitting ? 'Creando usuario...' : 'Crear usuario' }}
        </button>
      </form>

      <button
        v-if="isRegistered"
        class="submit-button"
        type="button"
        @click="router.push('/login')"
      >
        Ir al inicio de sesión
      </button>

      <p class="register-prompt">
        ¿Ya tienes usuario?
        <button type="button" class="register-link" @click="router.push('/login')">
          Inicia sesión
        </button>
      </p>

      <p class="login-note">Descubre, explora y evalúa películas para saber cuáles realmente valen la pena ver.</p>
    </section>

    <aside class="login-art" aria-label="Colección de películas">
      <div class="art-copy">
        <p class="eyebrow">Tu próxima función</p>
        <p class="art-title">Historias que merecen otra reproducción.</p>
      </div>
    </aside>
  </main>
</template>

<style scoped>
.login-shell {
  min-height: 100vh;
  display: grid;
  grid-template-columns: 1fr 1fr;
  background: #f5f6f8;
}

.login-panel {
  width: 100%;
  max-width: 520px;
  margin: auto;
  padding: 50px;
}

.brand-mark {
  width: 60px;
  height: 60px;
  margin-bottom: 22px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 14px;
  background: #111827;
  color: #ffffff;
  font-size: 28px;
  font-weight: 800;
}

.eyebrow {
  margin: 0 0 8px;
  color: #2563eb;
  font-size: 13px;
  font-weight: 800;
  letter-spacing: 1px;
  text-transform: uppercase;
}

.login-panel h1 {
  margin: 0 0 12px;
  color: #111827;
  font-size: 38px;
  font-weight: 800;
  line-height: 1.15;
}

.login-intro {
  max-width: 420px;
  margin: 0 0 35px;
  color: #6b7280;
  font-size: 16px;
  line-height: 1.6;
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.login-form label {
  margin-top: 10px;
  color: #374151;
  font-size: 14px;
  font-weight: 700;
}

.login-form input {
  width: 100%;
  padding: 13px 14px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  outline: none;
  background: #ffffff;
  color: #111827;
  font-size: 15px;
  transition: border-color 0.2s ease, box-shadow 0.2s ease;
}

.login-form input::placeholder {
  color: #9ca3af;
}

.login-form input:focus {
  border-color: #2563eb;
  box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.12);
}

.form-error,
.success-message {
  margin: 12px 0 4px;
  padding: 11px 13px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 600;
}

.form-error {
  background: #fef2f2;
  color: #b91c1c;
}

.success-message {
  display: flex;
  flex-direction: column;
  gap: 5px;
  background: #ecfdf5;
  color: #047857;
}

.submit-button {
  width: 100%;
  margin-top: 18px;
  padding: 13px;
  border: none;
  border-radius: 8px;
  background: #2563eb;
  color: #ffffff;
  font-size: 15px;
  font-weight: 700;
  cursor: pointer;
  transition: background 0.2s ease, transform 0.1s ease;
}

.submit-button:hover:not(:disabled) {
  background: #1d4ed8;
}

.submit-button:active:not(:disabled) {
  transform: scale(0.99);
}

.submit-button:disabled {
  cursor: not-allowed;
  opacity: 0.7;
}

.register-prompt {
  margin: 22px 0 0;
  color: #6b7280;
  font-size: 14px;
  text-align: center;
}

.register-link {
  padding: 0;
  border: none;
  background: transparent;
  color: #2563eb;
  font-weight: 700;
}

.register-link:hover {
  color: #1d4ed8;
  text-decoration: underline;
}

.login-note {
  margin: 25px 0 0;
  color: #9ca3af;
  font-size: 13px;
  line-height: 1.5;
}

.login-art {
  position: relative;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 60px;
  overflow: hidden;
  background: #111827;
}

.login-art::before {
  content: '';
  position: absolute;
  width: 500px;
  height: 500px;
  top: -180px;
  right: -150px;
  border-radius: 50%;
  background: #2563eb;
  opacity: 0.18;
}

.login-art::after {
  content: '🎬';
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -55%);
  font-size: 150px;
  opacity: 0.08;
}

.art-copy {
  position: relative;
  z-index: 2;
  max-width: 500px;
}

.login-art .eyebrow {
  color: #60a5fa;
}

.art-title {
  margin: 0;
  color: #ffffff;
  font-size: 42px;
  font-weight: 800;
  line-height: 1.15;
}

@media (max-width: 800px) {
  .login-shell {
    grid-template-columns: 1fr;
  }

  .login-panel {
    max-width: 520px;
    padding: 40px 25px;
  }

  .login-art {
    display: none;
  }
}

@media (max-width: 500px) {
  .login-panel h1 {
    font-size: 32px;
  }
}
</style>
