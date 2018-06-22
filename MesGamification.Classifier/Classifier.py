## IMPORTS ##
import sys
import scipy
import numpy
import matplotlib
import pandas
import sklearn
import json
import os
from sklearn import model_selection
import matplotlib.pyplot as plt
from sklearn.neighbors import KNeighborsClassifier
from sklearn.naive_bayes import GaussianNB
from sklearn.svm import SVC
from sklearn.metrics import accuracy_score
from sklearn.metrics import confusion_matrix
from sklearn.metrics import classification_report

## CARREGAR ARQUIVO ##
local = os.path.dirname(os.path.realpath(__file__))
arquivo = "Dataset.csv"
# arquivo = "iris.csv"
local_arquivo = local + '/' + arquivo
nomes = ['UserId', 'Operacoes', 'Consultas', 'Cadastros', 'Classificacao']
dataset = pandas.read_csv(local_arquivo, names=nomes)

## VALIDACOES DO MODELO DE DADOS ##
# print(dataset.groupby('Classificacao').size())
# dataset.plot(kind='box', subplots=True, layout=(2,2), sharex=False, sharey=False)
# dataset.hist()
# plt.show()

## DIVISAO DA MASSA DE DADOS ##
valores = dataset.values
X = valores[:,1:4] # Na utilização para o Iris Dataset setar esse valor como valores[:,0:4]
Y = valores[:,4]
divisao_treino = 0.15
semente = 7
X_treino, X_validacao, Y_treino, Y_validacao = model_selection.train_test_split(X, Y, test_size=divisao_treino, random_state=semente)
scoring = 'accuracy'

## ALGORITMOS ##
modelos = []
modelos.append(('KNN', KNeighborsClassifier()))
modelos.append(('NB', GaussianNB()))
modelos.append(('SVM', SVC()))

## Executa cada algoritmo e armazena o resultado ##
resultados = []
nomes = []
for nome, modelo in modelos:
	kfold = model_selection.KFold(n_splits=10, random_state=semente)
	cv_resultados = model_selection.cross_val_score(modelo, X_treino, Y_treino, cv=kfold, scoring=scoring)
	resultados.append(cv_resultados)
	nomes.append(nome)
	msg = "%s: %f (%f)" % (nome, cv_resultados.mean(), cv_resultados.std())
	print(msg)

fig = plt.figure()
fig.suptitle('Algorithm Comparison')
ax = fig.add_subplot(111)
plt.boxplot(resultados)
ax.set_xticklabels(nomes)
plt.show()

# KNN
# knn = KNeighborsClassifier()
# knn.fit(X_treino, Y_treino)
# predictions = knn.predict(X_validacao)
# print('\nAccuracy: ', accuracy_score(Y_validacao, predictions))
# print('\nConfusion: \n', confusion_matrix(Y_validacao, predictions))
# print('\nClassification: \n', classification_report(Y_validacao, predictions))

# NAIVE BAYES
nb = GaussianNB()
nb.fit(X_treino, Y_treino)
predictions = nb.predict(X_validacao)
print('\nAccuracy: ', accuracy_score(Y_validacao, predictions))
print('\nConfusion: \n', confusion_matrix(Y_validacao, predictions))
print('\nClassification: \n', classification_report(Y_validacao, predictions))

# # SVM
# svm = SVC()
# svm.fit(X_treino, Y_treino)
# predictions = svm.predict(X_validacao)
# print('\nAccuracy: ', accuracy_score(Y_validacao, predictions))
# print('\nConfusion: \n', confusion_matrix(Y_validacao, predictions))
# print('\nClassification: \n', classification_report(Y_validacao, predictions))