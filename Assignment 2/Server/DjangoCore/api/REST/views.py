from django.http import JsonResponse
from django.shortcuts import render
from django.views.decorators.csrf import csrf_exempt
from rest_framework.response import Response
import ServerClass
import json

# Create your views here.
@csrf_exempt
def change_color(request):
    color_data = json.loads(request.body).get("color")
    ServerClass.color = int(color_data)
    print("Empfangene Farbe (REST):", color_data)
    return  JsonResponse({'color': f"{color_data}"})

@csrf_exempt
def login(request):
    name = json.loads(request.body).get("name")
    uid = json.loads(request.body).get("uid")

    print("Name: ", name)
    print("UID: ", uid)
    return  JsonResponse({'status': "ok"})