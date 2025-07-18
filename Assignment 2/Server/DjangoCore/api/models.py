from django.db import models
from django.conf import settings
from django_mongodb_backend.fields import *
from django_mongodb_backend.models import *
# Create your models here.

##################################-Embedded-##################################

class Card(EmbeddedModel):
    id = ObjectIdAutoField(primary_key=True)
    type = models.IntegerField()
    lvl = models.IntegerField()
    count = models.IntegerField()

class Character(EmbeddedModel):
    id = ObjectIdAutoField(primary_key=True)
    type = models.IntegerField()
    lvl = models.IntegerField()
    hp = models.IntegerField()
    deck = EmbeddedModelArrayField(embedded_model=Card, null=True, blank=True)

class Enemy(EmbeddedModel):
    id = ObjectIdAutoField(primary_key=True)
    type = models.IntegerField()
    lvl = models.IntegerField()
    hp = models.IntegerField()
    deck = EmbeddedModelArrayField(embedded_model=Card, null=True, blank=True)

##################################-Actual-##################################

class User(models.Model):
    id = ObjectIdAutoField(primary_key=True)
    name = models.CharField(max_length=50)
    lvl = models.IntegerField()
    gold = models.IntegerField()
    upgradePoints = models.IntegerField()
    selectedCharacter = models.IntegerField()
    cards = EmbeddedModelArrayField(embedded_model=Card, null=True, blank=True)
    characters = EmbeddedModelArrayField(embedded_model=Character, null=True, blank=True)
    friends = ArrayField(models.IntegerField(), null=True, blank=True)

    def __str__(self):
        return self.name


class Party(models.Model):
    pid = ObjectIdAutoField(primary_key=True)
    hp = models.IntegerField()
    shield = models.IntegerField()
    members = ArrayField(ObjectIdField, null=True, blank=True)
    PoIIds = ArrayField(models.IntegerField(), null=True, blank=True)


class BattleArena(models.Model):
    id = ObjectIdAutoField(primary_key=True)
    pid = ObjectIdField()
    enemy = EmbeddedModelField(embedded_model=Enemy, null=True, blank=True)
    rewardGold = models.IntegerField()
    rewardCards = EmbeddedModelArrayField(embedded_model=Card, null=True, blank=True)
    rewardUpgradePoints = models.IntegerField()
    teamHP = models.IntegerField()
    teamShield = models.IntegerField()
    playerChecks = ArrayField(models.BooleanField(), null=True, blank=True)
    playerCards = EmbeddedModelArrayField(embedded_model=Card, null=True, blank=True)
    enemyCards = EmbeddedModelArrayField(embedded_model=Card, null=True, blank=True)
    battleState = models.IntegerField()
