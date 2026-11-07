```vue
<script setup lang="ts">
import { reactive, watch } from 'vue'
import {
  Film,
  User,
  Users,
  Tags,
  Calendar,
  Clock,
  Star,
  Languages,
  Globe,
  Image,
  FileText,
  Save,
} from 'lucide-vue-next'
import type { Movie } from '../types/movie'

const props = defineProps<{
  initialMovie?: Movie
  editing?: boolean
}>()

const emit = defineEmits<{
  submit: [movie: Omit<Movie, 'id'>]
  cancel: []
}>()

const movie = reactive<Omit<Movie, 'id'>>({
  titulo: '',
  director: '',
  reparto: '',
  genero: '',
  clasificacion_edad: '',
  fecha_estreno: '',
  duracion: 0,
  sinopsis: '',
  poster_url: '',
  calificacion: 0,
  idioma: '',
  pais: '',
})

const fillForm = (data?: Movie) => {
  if (!data) return

  movie.titulo = data.titulo
  movie.director = data.director
  movie.reparto = data.reparto
  movie.genero = data.genero
  movie.clasificacion_edad = data.clasificacion_edad
  movie.fecha_estreno = data.fecha_estreno
  movie.duracion = data.duracion
  movie.sinopsis = data.sinopsis
  movie.poster_url = data.poster_url
  movie.calificacion = data.calificacion
  movie.idioma = data.idioma
  movie.pais = data.pais
}

watch(
  () => props.initialMovie,
  (newMovie) => {
    fillForm(newMovie)
  },
  { immediate: true },
)

const submitForm = () => {
  if (movie.calificacion < 0 || movie.calificacion > 10) {
    alert('La calificación debe estar entre 0 y 10.')
    return
  }

  emit('submit', { ...movie })
}
</script>

<template>
  <form class="movie-form" @submit.prevent="submitForm">

    <div class="form-header">
      <div class="form-icon">
        <Film :size="26" />
      </div>

      <div>
        <h2>
          {{ editing ? 'Editar película' : 'Agregar película' }}
        </h2>

        <p>
          {{
            editing
              ? 'Modifica la información de la película.'
              : 'Completa la información para agregar una nueva película.'
          }}
        </p>
      </div>
    </div>

    <div class="form-grid">

      <div class="form-group">
        <label for="titulo">
          <Film :size="16" />
          Título
        </label>

        <input
          id="titulo"
          v-model="movie.titulo"
          type="text"
          placeholder="Ej. Interstellar"
          required
        />
      </div>

      <div class="form-group">
        <label for="director">
          <User :size="16" />
          Director
        </label>

        <input
          id="director"
          v-model="movie.director"
          type="text"
          placeholder="Ej. Christopher Nolan"
          required
        />
      </div>

      <div class="form-group full-width">
        <label for="reparto">
          <Users :size="16" />
          Reparto
        </label>

        <input
          id="reparto"
          v-model="movie.reparto"
          type="text"
          placeholder="Ej. Matthew McConaughey, Anne Hathaway"
        />
      </div>

      <div class="form-group">
        <label for="genero">
          <Tags :size="16" />
          Género
        </label>

        <input
          id="genero"
          v-model="movie.genero"
          type="text"
          placeholder="Ej. Ciencia ficción"
          required
        />
      </div>

      <div class="form-group">
        <label for="clasificacion">
          <Film :size="16" />
          Clasificación por edad
        </label>

        <input
          id="clasificacion"
          v-model="movie.clasificacion_edad"
          type="text"
          placeholder="Ej. 13+"
        />
      </div>

      <div class="form-group">
        <label for="fecha">
          <Calendar :size="16" />
          Fecha de estreno
        </label>

        <input
          id="fecha"
          v-model="movie.fecha_estreno"
          type="date"
        />
      </div>

      <div class="form-group">
        <label for="duracion">
          <Clock :size="16" />
          Duración
        </label>

        <div class="input-with-suffix">
          <input
            id="duracion"
            v-model.number="movie.duracion"
            type="number"
            min="0"
            placeholder="120"
          />
          <span>min</span>
        </div>
      </div>

      <div class="form-group">
        <label for="calificacion">
          <Star :size="16" />
          Calificación
        </label>

        <div class="input-with-suffix">
          <input
            id="calificacion"
            v-model.number="movie.calificacion"
            type="number"
            min="0"
            max="10"
            step="0.1"
            placeholder="8.5"
          />
          <span>/ 10</span>
        </div>
      </div>

      <div class="form-group">
        <label for="idioma">
          <Languages :size="16" />
          Idioma
        </label>

        <input
          id="idioma"
          v-model="movie.idioma"
          type="text"
          placeholder="Ej. Inglés"
        />
      </div>

      <div class="form-group">
        <label for="pais">
          <Globe :size="16" />
          País
        </label>

        <input
          id="pais"
          v-model="movie.pais"
          type="text"
          placeholder="Ej. Estados Unidos"
        />
      </div>

      <div class="form-group full-width">
        <label for="poster">
          <Image :size="16" />
          URL del poster
        </label>

        <input
          id="poster"
          v-model="movie.poster_url"
          type="url"
          placeholder="https://..."
        />
      </div>

      <div class="form-group full-width">
        <label for="sinopsis">
          <FileText :size="16" />
          Sinopsis
        </label>

        <textarea
          id="sinopsis"
          v-model="movie.sinopsis"
          placeholder="Escribe una breve descripción de la película..."
        ></textarea>
      </div>

    </div>

    <div class="form-footer">
      <div class="form-actions">
        <button
          type="button"
          class="cancel-button"
          @click="emit('cancel')"
        >
          Cancelar
        </button>

        <button type="submit" class="submit-button">
          <Save :size="18" />
          {{ editing ? 'Actualizar película' : 'Guardar película' }}
        </button>
      </div>
    </div>

  </form>
</template>

<style scoped>
.movie-form {
  width: 100%;
  padding: 32px;
  border: 1px solid #e5e7eb;
  border-radius: 16px;
  background: #ffffff;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.07);
}

.form-header {
  display: flex;
  align-items: center;
  gap: 15px;
  padding-bottom: 25px;
  margin-bottom: 25px;
  border-bottom: 1px solid #e5e7eb;
}

.form-icon {
  width: 52px;
  height: 52px;
  flex-shrink: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 12px;
  background: #eff6ff;
  color: #2563eb;
}

.form-header h2 {
  margin: 0 0 5px;
  color: #111827;
  font-size: 24px;
  font-weight: 800;
}

.form-header p {
  margin: 0;
  color: #6b7280;
  font-size: 14px;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 7px;
}

.full-width {
  grid-column: 1 / -1;
}

.form-group label {
  display: flex;
  align-items: center;
  gap: 6px;
  color: #374151;
  font-size: 14px;
  font-weight: 700;
}

.form-group label svg {
  color: #2563eb;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 12px 13px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  outline: none;
  background: #ffffff;
  color: #111827;
  font-size: 15px;
  transition:
    border-color 0.2s ease,
    box-shadow 0.2s ease;
}

.form-group input::placeholder,
.form-group textarea::placeholder {
  color: #9ca3af;
}

.form-group input:focus,
.form-group textarea:focus {
  border-color: #2563eb;
  box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.12);
}

.form-group textarea {
  min-height: 130px;
  resize: vertical;
}

.input-with-suffix {
  position: relative;
  display: flex;
  align-items: center;
}

.input-with-suffix input {
  padding-right: 55px;
}

.input-with-suffix span {
  position: absolute;
  right: 13px;
  color: #9ca3af;
  font-size: 13px;
  font-weight: 600;
  pointer-events: none;
}

.form-actions {
  display: flex;
  align-items: center;
  gap: 12px;
}

.cancel-button,
.submit-button {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 12px 22px;
  border-radius: 8px;
  font-size: 15px;
  font-weight: 700;
  cursor: pointer;
  transition:
    background 0.2s ease,
    transform 0.1s ease;
}

.cancel-button {
  border: 1px solid #d1d5db;
  background: #ffffff;
  color: #374151;
}

.cancel-button:hover {
  background: #f3f4f6;
}

.cancel-button:active,
.submit-button:active {
  transform: scale(0.98);
}

.form-footer {
  justify-content: stretch;
  margin-top: 25px
}

.form-actions {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
}

.cancel-button,
.submit-button {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 12px 22px;
  border-radius: 8px;
  font-size: 15px;
  font-weight: 700;
  cursor: pointer;
  transition:
    background 0.2s ease,
    transform 0.1s ease;
}

.cancel-button {
  border: 1px solid #d1d5db;
  background: #ffffff;
  color: #374151;
}

.cancel-button:hover {
  background: #f3f4f6;
}

.cancel-button:active,
.submit-button:active {
  transform: scale(0.98);
}

.submit-button {
  border: none;
  background: #2563eb;
  color: #ffffff;
}

.submit-button:hover {
  background: #1d4ed8;
}

@media (max-width: 700px) {
  .movie-form {
    padding: 22px;
  }

  .form-grid {
    grid-template-columns: 1fr;
  }

  .full-width {
    grid-column: auto;
  }

  .form-actions {
    flex-direction: column-reverse;
  }

  .cancel-button,
  .submit-button {
    width: 100%;
  }
}
</style>
