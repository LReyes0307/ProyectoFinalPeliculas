<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import Navbar from '../components/Navbar.vue'
import MovieForm from '../components/MovieForm.vue'
import { useMoviesStore } from '../stores/movies'
import type { Movie } from '../types/movie'
import { CheckCircle } from 'lucide-vue-next'

const route = useRoute()
const router = useRouter()
const moviesStore = useMoviesStore()
const showSuccessModal = ref(false)
const successMessage = ref('')
const errorMessage = ref('')

const editing = computed(() => {
  return Boolean(route.params.id)
})

const movieToEdit = computed(() => {
  if (!editing.value) return undefined

  return moviesStore.getMovieById(Number(route.params.id))
})

onMounted(async () => {
  if (editing.value && !movieToEdit.value) {
    try {
      await moviesStore.loadMovie(Number(route.params.id))
    } catch {
      errorMessage.value = 'No fue posible cargar la película.'
    }
  }
})

const saveMovie = async (movieData: Omit<Movie, 'id'>) => {
  errorMessage.value = ''

  try {
    if (editing.value && movieToEdit.value) {
      await moviesStore.updateMovie({
        id: movieToEdit.value.id,
        ...movieData,
      })

      successMessage.value = 'La película se actualizó correctamente.'
    } else {
      await moviesStore.addMovie(movieData)
      successMessage.value = 'La película se creó correctamente.'
    }

    showSuccessModal.value = true
  } catch {
    errorMessage.value = 'No fue posible guardar la película.'
  }
}

const closeSuccessModal = () => {
  showSuccessModal.value = false
  router.push('/movies')
}

const cancel = () => {
  if (editing.value && movieToEdit.value) {
    router.push(`/movies/${movieToEdit.value.id}`)
  } else {
    router.push('/movies')
  }
}
</script>

<template>
  <div class="create-page">
    <Navbar />

    <main class="create-content">
      <section class="page-header">
        <h1>
          {{ editing ? 'Editar película' : 'Agregar película' }}
        </h1>

        <p>
          {{
            editing
              ? 'Modifica la información de esta película.'
              : 'Agrega la información de una película para que los usuarios puedan conocerla y decidir si vale la pena verla.'
          }}
        </p>
      </section>

      <MovieForm
        :initial-movie="movieToEdit"
        :editing="editing"
        @submit="saveMovie"
        @cancel="cancel"
      />

      <p v-if="errorMessage" class="form-error" role="alert">
        {{ errorMessage }}
      </p>

      <div
        v-if="showSuccessModal"
        class="modal-overlay"
        @click.self="closeSuccessModal"
      >
        <div class="modal">
          <div class="modal-icon success-icon">
            <CheckCircle :size="30" />
          </div>

          <h2>
            {{ editing ? '¡Película actualizada!' : '¡Película creada!' }}
          </h2>

          <p>
            {{ successMessage }}
          </p>

          <div class="modal-actions">
            <button
              class="confirm-success-modal"
              @click="closeSuccessModal"
            >
              Aceptar
            </button>
          </div>
        </div>
      </div>
    </main>
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

.success-icon {
  background: #dcfce7;
  color: #16a34a;
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

.modal-actions {
  display: flex;
  justify-content: flex-end;
  margin-top: 25px;
}

.confirm-success-modal {
  padding: 11px 22px;
  border: none;
  border-radius: 8px;
  background: #2563eb;
  color: #ffffff;
  font-size: 14px;
  font-weight: 700;
  cursor: pointer;
  transition: background 0.2s ease;
}

.confirm-success-modal:hover {
  background: #1d4ed8;
}

@media (max-width: 500px) {
  .modal {
    padding: 25px;
  }

  .confirm-success-modal {
    width: 100%;
  }
}

.create-page {
  min-height: 100vh;
  background: #f5f6f8;
}

.create-content {
  max-width: 900px;
  margin: 0 auto;
  padding: 40px 25px;
}

.page-header {
  margin-bottom: 25px;
}

.page-header h1 {
  margin: 0 0 8px;
  color: #111827;
  font-size: 32px;
  font-weight: 800;
}

.page-header p {
  margin: 0;
  max-width: 700px;
  color: #6b7280;
  line-height: 1.6;
}
</style>