﻿using System;
using System.Collections.Generic;
using System.Text;
using Tensorflow.Keras.ArgsDefinition;
using static Tensorflow.Binding;

namespace Tensorflow.Keras.Engine.DataAdapters
{
    /// <summary>
    /// Adapter that handles Tensor-like objects, e.g. EagerTensor and NumPy.
    /// </summary>
    public class TensorLikeDataAdapter : IDataAdapter
    {
        TensorLikeDataAdapterArgs args;
        int _size;
        int _batch_size;
        int num_samples;
        int num_full_batches;

        public TensorLikeDataAdapter(TensorLikeDataAdapterArgs args)
        {
            this.args = args;
            _process_tensorlike();
            num_samples = args.X.shape[0];
            var batch_size = args.BatchSize;
            _batch_size = batch_size;
            _size = Convert.ToInt32(Math.Ceiling(num_samples / (batch_size + 0f)));
            num_full_batches = num_samples / batch_size;
            var _partial_batch_size = num_samples % batch_size;

            var indices_dataset = tf.data.Dataset.range(1);
            indices_dataset = indices_dataset.repeat();
            indices_dataset = indices_dataset.map(permutation).prefetch(1);
            indices_dataset = indices_dataset.flat_map(slice_batch_indices);
        }

        Tensor permutation(Tensor tensor)
        {
            var indices = math_ops.range(num_samples, dtype: dtypes.int64);
            if (args.Shuffle)
                indices = random_ops.random_shuffle(indices);
            return indices;
        }

        /// <summary>
        /// Convert a Tensor of indices into a dataset of batched indices.
        /// </summary>
        /// <param name="tensor"></param>
        /// <returns></returns>
        IDatasetV2 slice_batch_indices(Tensor indices)
        {
            var num_in_full_batch = num_full_batches * _batch_size;
            var first_k_indices = array_ops.slice(indices, new int[] { 0 }, new int[] { num_in_full_batch });
            first_k_indices = array_ops.reshape(first_k_indices, new int[] { num_full_batches, _batch_size });
            var flat_dataset = tf.data.Dataset.from_tensor_slices(first_k_indices);

            return flat_dataset;
        }

        void slice_inputs(IDatasetV2 indices_dataset, Tensor x, Tensor y)
        {
            var dataset = tf.data.Dataset.from_tensor(x, y);
        }

        public bool CanHandle(Tensor x, Tensor y = null)
        {
            throw new NotImplementedException();
        }

        void _process_tensorlike()
        {
        }
    }
}
