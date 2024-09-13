using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KahuInteractive.UIHelpers
{

public static class ContentDistributor
{
    public enum Direction
    {
        TopToBottom,
        LeftToRight
    }

    public static void DistributeContent(List<RectTransform> contents, RectTransform within, Direction direction)
    {
        int numberOfContent = contents.Count;

        float totalSpace = 0f;

        float yHeight = within.rect.max.y - within.rect.min.y;
        float xWidth = within.rect.max.x - within.rect.min.x;

        if (direction == Direction.TopToBottom)
        {
            totalSpace = yHeight;
        }
        else if (direction == Direction.LeftToRight)
        {
            totalSpace = xWidth;
        }

        float spaceBetweenContent = totalSpace / (numberOfContent - 1.0f);

        for (int i = 0; i < numberOfContent; i++)
        {
            RectTransform content = contents[i];

            float xPos = 0f;
            float yPos = 0f;

            if (direction == Direction.TopToBottom)
            {
                yPos = - (i * spaceBetweenContent);
            }
            else if (direction == Direction.LeftToRight)
            {
                xPos = - (i * spaceBetweenContent);
            }

            content.anchoredPosition = new Vector2(xPos, yPos);
        }

    }

}


}