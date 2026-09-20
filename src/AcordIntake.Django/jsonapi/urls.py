from django.urls import path

from .views import json_intake

urlpatterns = [
    path('intake/json', json_intake, name='json_intake'),
]
