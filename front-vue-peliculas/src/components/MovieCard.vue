<script setup lang="ts">
import { useRouter } from 'vue-router'
import {Flame, ThumbsUp, CircleHelp, CircleX, Star,} from 'lucide-vue-next'
import type { Movie } from '../types/movie'

defineProps<{
  movie: Movie
}>()

const router = useRouter()

const getRecommendation = (calificacion: number) => {
  if (calificacion >= 8) {
    return {
      text: 'Muy recomendada',
      icon: Flame,
      class: 'recommendation-excellent',
    }
  }

  if (calificacion >= 6) {
    return {
      text: 'Vale la pena',
      icon: ThumbsUp,
      class: 'recommendation-good',
    }
  }

  if (calificacion >= 4) {
    return {
      text: 'Regular',
      icon: CircleHelp,
      class: 'recommendation-regular',
    }
  }

  return {
    text: 'No recomendada',
    icon: CircleX,
    class: 'recommendation-bad',
  }
}

const viewDetails = (id: number) => {
  router.push(`/movies/${id}`)
}
</script>

<template>
  <article class="movie-card">
    <img
      :src="movie.poster_url"
      :alt="`Poster de ${movie.titulo}`"
      class="movie-poster"
    />

    <div class="movie-info">
      <h2>{{ movie.titulo }}</h2>

      <p class="genre">{{ movie.genero }}</p>

      <div class="rating">
        <Star :size="18" />

        <span>{{ movie.calificacion }}/10</span>
      </div>

      <div
        class="recommendation"
        :class="getRecommendation(movie.calificacion).class"
      >
        <component
          :is="getRecommendation(movie.calificacion).icon"
          :size="18"
        />

        <span>
          {{ getRecommendation(movie.calificacion).text }}
        </span>
      </div>

      <button
            class="details-button"
            @click="viewDetails(movie.id)"
            >
            Ver detalles
        </button>
    </div>
  </article>
</template>

<style scoped>
.movie-card {
  width: 260px;
  overflow: hidden;
  border-radius: 12px;
  background: #ffffff;
  border: 1px solid #e5e7eb;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.movie-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.12);
}

.movie-poster {
  width: 100%;
  height: 360px;
  object-fit: cover;
  display: block;
}

.movie-info {
  padding: 18px;
}

.movie-info h2 {
  margin: 0 0 6px;
  color: #111827;
  font-size: 20px;
  font-weight: 700;
}

.genre {
  margin: 0 0 12px;
  color: #6b7280;
  font-size: 14px;
}

.rating {
  display: flex;
  align-items: center;
  gap: 7px;
  margin-bottom: 8px;
  color: #ca8a04;
  font-weight: 700;
}

.recommendation {
  display: flex;
  align-items: center;
  gap: 7px;
  margin: 0 0 16px;
  font-weight: 600;
}

.recommendation-excellent {
  color: #ea580c;
}

.recommendation-good {
  color: #16a34a;
}

.recommendation-regular {
  color: #ca8a04;
}

.recommendation-bad {
  color: #dc2626;
}

.details-button {
  width: 100%;
  padding: 10px;
  border: none;
  border-radius: 8px;
  background: #2563eb;
  color: #ffffff;
  font-weight: 600;
  transition: background 0.2s ease;
}

.details-button:hover {
  background: #1d4ed8;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  margin-top: 25px;
  padding-top: 20px;
  border-top: 1px solid #e5e7eb;
}

.cancel-button,
.submit-button {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 7px;
  padding: 11px 20px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
}

.cancel-button {
  border: 1px solid #d1d5db;
  background: #ffffff;
  color: #374151;
}

.cancel-button:hover {
  background: #f3f4f6;
}

.submit-button {
  border: none;
  background: #2563eb;
  color: #ffffff;
}

.submit-button:hover {
  background: #1d4ed8;
}
</style>