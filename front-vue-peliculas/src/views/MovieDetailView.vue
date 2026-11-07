<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import Navbar from '../components/Navbar.vue'
import { useMoviesStore } from '../stores/movies'
import { Trash2 } from 'lucide-vue-next'

const route = useRoute()
const router = useRouter()
const moviesStore = useMoviesStore()
const showDeleteModal = ref(false)
const errorMessage = ref('')

const askDelete = () => {
  showDeleteModal.value = true
}

const cancelDelete = () => {
  showDeleteModal.value = false
}

const confirmDelete = async () => {
  if (!movie.value) return

  try {
    await moviesStore.deleteMovie(movie.value.id)
    showDeleteModal.value = false
    await router.push('/movies')
  } catch {
    errorMessage.value = 'No fue posible eliminar la película.'
  }
}

const movie = computed(() => {
  return moviesStore.getMovieById(Number(route.params.id))
})

onMounted(async () => {
  if (!movie.value) {
    try {
      await moviesStore.loadMovie(Number(route.params.id))
    } catch {
      errorMessage.value = 'No fue posible cargar la película.'
    }
  }
})

const editMovie = () => {
  router.push(`/movies/${movie.value?.id}/edit`)
}

const goBack = () => {
  router.push('/movies')
}

const getRecommendation = (calificacion: number) => {
  if (calificacion >= 8) return '🔥 Muy recomendada'
  if (calificacion >= 6) return '👍 Vale la pena'
  if (calificacion >= 4) return '🤔 Regular'
  return '❌ No recomendada'
}
</script>

<template>
  <div class="detail-page">
    <Navbar />

    <main v-if="movie" class="detail-content">
      <p v-if="errorMessage" class="form-error" role="alert">
        {{ errorMessage }}
      </p>
      <button class="back-button" @click="goBack">
        ← Volver a películas
      </button>

      <section class="movie-detail">
        <img
          :src="movie.poster_url"
          :alt="`Poster de ${movie.titulo}`"
          class="movie-poster"
        />

        <div class="movie-information">
          <h1>{{ movie.titulo }}</h1>

          <p class="recommendation">
            {{ getRecommendation(movie.calificacion) }}
          </p>

          <div class="rating">
            ⭐ {{ movie.calificacion }} / 10
          </div>

          <div class="details">
            <p>
              <strong>Director:</strong>
              {{ movie.director }}
            </p>

            <p>
              <strong>Reparto:</strong>
              {{ movie.reparto }}
            </p>

            <p>
              <strong>Género:</strong>
              {{ movie.genero }}
            </p>

            <p>
              <strong>Clasificación:</strong>
              {{ movie.clasificacion_edad }}
            </p>

            <p>
              <strong>Fecha de estreno:</strong>
              {{ movie.fecha_estreno }}
            </p>

            <p>
              <strong>Duración:</strong>
              {{ movie.duracion }} minutos
            </p>

            <p>
              <strong>Idioma:</strong>
              {{ movie.idioma }}
            </p>

            <p>
              <strong>País:</strong>
              {{ movie.pais }}
            </p>
          </div>

          <div class="synopsis">
            <h2>Sinopsis</h2>
            <p>{{ movie.sinopsis }}</p>
          </div>

          <div class="actions">
            <button class="edit-button" @click="editMovie">
              ✏️ Editar
            </button>

            <button class="delete-button" @click="askDelete">
              🗑️ Eliminar
            </button>
          </div>
        </div>
      </section>
    </main>

    <main v-else class="not-found">
      <h1>Película no encontrada</h1>

      <button @click="goBack">
        Volver a películas
      </button>
    </main>
  </div>

  <div
  v-if="showDeleteModal"
  class="modal-overlay"
  @click.self="cancelDelete"
>
  <div class="modal">
    <div class="modal-icon delete-icon">
      <Trash2 :size="28" />
    </div>

    <h2>¿Eliminar película?</h2>

    <p>
      ¿Está seguro de que desea eliminar
      <strong>{{ movie?.titulo }}</strong>?
      Esta acción no se puede deshacer.
    </p>

    <div class="modal-actions">
      <button
        class="cancel-modal"
        @click="cancelDelete"
      >
        Cancelar
      </button>

      <button
        class="confirm-delete-modal"
        @click="confirmDelete"
      >
        <Trash2 :size="17" />
        Eliminar
      </button>
    </div>
  </div>
</div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  z-index: 1000;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
  background: rgba(17, 24, 39, 0.55);
}

.modal {
  width: 100%;
  max-width: 430px;
  padding: 30px;
  border-radius: 16px;
  background: #ffffff;
  box-shadow: 0 20px 50px rgba(0, 0, 0, 0.2);
  text-align: center;
}

.modal-icon {
  width: 58px;
  height: 58px;
  margin: 0 auto 18px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
}

.delete-icon {
  background: #fee2e2;
  color: #dc2626;
}

.modal h2 {
  margin: 0 0 10px;
  color: #111827;
  font-size: 23px;
  font-weight: 800;
}

.modal p {
  margin: 0;
  color: #6b7280;
  font-size: 15px;
  line-height: 1.6;
}

.modal p strong {
  color: #374151;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 25px;
}

.cancel-modal,
.confirm-delete-modal {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 7px;
  padding: 11px 18px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 700;
  cursor: pointer;
  transition: background 0.2s ease;
}

.cancel-modal {
  border: 1px solid #d1d5db;
  background: #ffffff;
  color: #374151;
}

.cancel-modal:hover {
  background: #f3f4f6;
}

.confirm-delete-modal {
  border: none;
  background: #dc2626;
  color: #ffffff;
}

.confirm-delete-modal:hover {
  background: #b91c1c;
}

@media (max-width: 500px) {
  .modal {
    padding: 25px;
  }

  .modal-actions {
    flex-direction: column-reverse;
  }

  .cancel-modal,
  .confirm-delete-modal {
    width: 100%;
  }
}

.detail-content {
  max-width: 1100px;
  margin: 0 auto;
  padding: 35px 25px;
}

.back-button {
  margin-bottom: 25px;
  padding: 10px 15px;
  border: none;
  border-radius: 8px;
  background: transparent;
  color: #2563eb;
  font-weight: 600;
}

.movie-detail {
  display: grid;
  grid-template-columns: 320px 1fr;
  gap: 40px;
  padding: 30px;
  border-radius: 16px;
  background: #ffffff;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.08);
}

.movie-poster {
  width: 100%;
  border-radius: 12px;
}

.movie-information h1 {
  margin-top: 0;
  margin-bottom: 10px;
  color: #111827;
  font-size: 36px;
}

.recommendation {
  color: #166534;
  font-size: 18px;
  font-weight: 700;
}

.rating {
  margin: 15px 0 25px;
  font-size: 20px;
  font-weight: 700;
}

.details p {
  margin: 10px 0;
  color: #4b5563;
}

.details strong {
  color: #111827;
}

.synopsis {
  margin-top: 25px;
}

.synopsis h2 {
  color: #111827;
}

.synopsis p {
  color: #4b5563;
  line-height: 1.6;
}

.actions {
  display: flex;
  gap: 12px;
  margin-top: 30px;
}

.edit-button,
.delete-button {
  padding: 12px 20px;
  border: none;
  border-radius: 8px;
  color: white;
  font-weight: 600;
}

.edit-button {
  background: #2563eb;
}

.edit-button:hover {
  background: #1d4ed8;
}

.delete-button {
  background: #dc2626;
}

.delete-button:hover {
  background: #b91c1c;
}

.not-found {
  padding: 80px 20px;
  text-align: center;
}

@media (max-width: 700px) {
  .movie-detail {
    grid-template-columns: 1fr;
  }
}
</style>