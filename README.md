# Clevernce-M3BLACK
add support device M3BLACK to cleverence (rev eng win ce app)

Решение проблемы поддержки cleverence сканера M3BLACK в cleverence, те клиент cleverence(win ce) при нажатии на клавишу начать сканирование не открывал порт тсд для работы сканера(софт запускается сканер не стартует).

1. установить с пк mobile smarts поставку для модели M3T6700

2. после установки на тсд в папку mobile smarts(задается при установке с пк) скопировать файлы:

Cleverence.Compact.Core.M3T6700.dll(с заменой)

Imager.dll(новый)

ImagerNet.dll(новый)

M3System.dll(новый)

M3SystemNet.dll(новый)

3. (не обязательно) удалить:

ScannerNet.dll

Scanner.dll

MCSSLib6500.dll

исходники в src