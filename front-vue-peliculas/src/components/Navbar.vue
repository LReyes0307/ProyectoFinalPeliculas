<script setup lang="ts">
import { Film, Plus, LogOut } from 'lucide-vue-next'
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'


const router = useRouter()
const authStore = useAuthStore()
const route = useRoute()

const showLogoutModal = ref(false)

const goToMovies = () => {
  router.push('/movies')
}

const goToCreateMovie = () => {
  router.push('/movies/create')
}

const askLogout = () => {
  showLogoutModal.value = true
}

const cancelLogout = () => {
  showLogoutModal.value = false
}

const confirmLogout = () => {
  authStore.logout()
  showLogoutModal.value = false
  router.push('/login')
}
</script>

<template>
  <nav class="navbar">
    <div class="logo" @click="goToMovies">
        <Film :size="24" />
        <span>MovieRadar</span>
    </div>

    <div class="nav-links">
      <button
        :class="{ active: route.path === '/movies' }"
        @click="goToMovies"
      >
        Películas
      </button>

      <button
        class="add-button"
        :class="{ active: route.path === '/movies/create' }"
        @click="goToCreateMovie"
      >
        <Plus :size="18" />
        Agregar película
      </button>

      <button class="logout-button" @click="askLogout">
        <LogOut :size="18" />
        Salir
       </button>
    </div>
  </nav>

  <div
    v-if="showLogoutModal"
    class="modal-overlay"
    @click.self="cancelLogout"
  >
    <div class="modal">
      <h2>¿Cerrar sesión?</h2>

      <p>
        ¿Está seguro de que desea salir de su cuenta?
      </p>

      <div class="modal-actions">
        <button
          class="cancel-modal"
          @click="cancelLogout"
        >
          Cancelar
        </button>

        <button
          class="confirm-modal"
          @click="confirmLogout"
        >
          Sí, salir
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.navbar {
  width: 100%;
  min-height: 70px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 40px;
  background: #111827;
  color: #ffffff;
}

.logo {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 22px;
  font-weight: 800;
  letter-spacing: 0.5px;
  cursor: pointer;
}

.nav-links {
  display: flex;
  align-items: center;
  gap: 10px;
}

.nav-links button {
  display: flex;
  align-items: center;
  gap: 7px;
  padding: 10px 14px;
  border: 1px solid transparent;
  border-radius: 8px;
  background: transparent;
  color: #ffffff;
  font-weight: 500;
  transition: all 0.2s ease;
}

/* Botón seleccionado */
.nav-links button.active {
  background: #374151;
  border-color: #4b5563;
}

/* Hover de botones normales */
.nav-links button:hover {
  background: #1f2937;
  border-color: #374151;
}

/* Agregar película */
.nav-links .add-button {
  background: transparent;
  color: #ffffff;
}

.nav-links .add-button:hover {
  background: #1f2937;
  border-color: #374151;
}

.nav-links .add-button.active {
  background: #2563eb;
  color: #ffffff;
  border-color: transparent;
}

.nav-links .add-button.active:hover {
  background: #1d4ed8;
  border-color: transparent;
}

/* Salir */
.nav-links .logout-button {
  color: #fca5a5;
}

.nav-links .logout-button:hover {
  background: #3f1d1d;
  border-color: #7f1d1d;
}

/* Modal */
.modal-overlay {
  position: fixed;
  inset: 0;
  z-index: 1000;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
  background: rgba(0, 0, 0, 0.55);
}

.modal {
  width: 100%;
  max-width: 420px;
  padding: 28px;
  border-radius: 14px;
  background: #ffffff;
  box-shadow: 0 15px 40px rgba(0, 0, 0, 0.25);
}

.modal h2 {
  margin: 0 0 10px;
  color: #111827;
}

.modal p {
  margin: 0 0 25px;
  color: #6b7280;
  line-height: 1.5;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.cancel-modal,
.confirm-modal {
  padding: 10px 18px;
  border: none;
  border-radius: 8px;
  font-weight: 600;
}

.cancel-modal {
  background: #e5e7eb;
  color: #374151;
}

.cancel-modal:hover {
  background: #d1d5db;
}

.confirm-modal {
  background: #dc2626;
  color: #ffffff;
}

.confirm-modal:hover {
  background: #b91c1c;
}
</style>