from django.db import models
from django.conf import settings
from django_mongodb_backend.fields import *
from django_mongodb_backend.models import *
# Create your models here.

class Card(EmbeddedModel):
    id = ObjectIdAutoField(primary_key=True)
    type = models.IntegerField()
    lvl = models.IntegerField()
    count = models.IntegerField()

class Character(EmbeddedModel):
    id = ObjectIdAutoField(primary_key=True)
    type = models.CharField(max_length = 20)
    lvl = models.IntegerField()
    baseHP = models.IntegerField()
    cards = ArrayField(EmbeddedModelField(Card), null=True, blank=True)


class User(models.Model):
    id = ObjectIdAutoField(primary_key=True)
    name = models.CharField(max_length=50)
    lvl = models.IntegerField()
    gold = models.IntegerField()
    upgradePoints = models.IntegerField()
    selectedCharacter = models.IntegerField()
    cards = ArrayField(EmbeddedModelField(Card), null=True, blank=True)
    characters = ArrayField(EmbeddedModelField(Character), null=True, blank=True)

    def __str__(self):
        return self.name