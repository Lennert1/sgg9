from django.urls import path
from views import change_color

urlpatterns = [
    path('change_color/', change_color),
]